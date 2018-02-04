using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Enums;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class TaskService : HostService<ITaskService>, ITaskService {

        #region [ .ctor ]

        public TaskService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) : base(servicesHost, unitOfWork, mapper) {
        }

        #endregion [ .ctor ]

        #region [ data ]

        /// <summary>
        ///     Gets projects, tasks, subtasks, comments by user </summary>
        /// <param name="user">user who requests the data</param>
        /// <remarks> if <see cref="historyDeep"/> is null then returns actual tasks else actual + history </remarks>
        /// <param name="historyDeep">minimum date of tasks</param>
        /// <param name="projects">out parameter</param>
        /// <param name="tasks">out parameter</param>
        /// <param name="historyFilters">out parameter</param>
        public void GetData(UserDto user, DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks,
            out List<DateTime> historyFilters) {

            var projectRep = UnitOfWork.GetRepository<Project>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var taskuserRep = UnitOfWork.GetRepository<TaskUser>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var commentRep = UnitOfWork.GetRepository<Comment>();

            var projectsQuery = projectRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var tasksQuery = taskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var subtasksQuery = subtaskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var commentsQuery = commentRep.SearchFor(e => e.CompanyId == user.CompanyId);

            historyFilters = commentsQuery.Select(e => e.Date).ToList()
                .Select(e => e.Date.AddDays(-e.Date.Day + 1)).Distinct().ToList();

            if (historyDeep == null) {
                var statuses = new List<TaskStatusEnum>() {
                    TaskStatusEnum.NotStarted,
                    TaskStatusEnum.InProgress
                }.Cast<int>().ToList();
                tasksQuery = tasksQuery.Where(e => statuses.Contains(e.Status));
            } else {
                var statuses = new List<TaskStatusEnum>() {
                    TaskStatusEnum.Done,
                    TaskStatusEnum.Failed,
                    TaskStatusEnum.Rejected,
                }.Cast<int>().ToList();
                // filtering by a task comments
                var tasksQuery1 = tasksQuery.Where(e => statuses.Contains(e.Status))
                     .Join(commentsQuery.Where(c => c.TaskId != null), t => t.EntityId, c => c.TaskId, (t, c) => new { t, c }).DefaultIfEmpty()
                     .Where(e => e.t != null && (e.c == null || e.c.Date >= historyDeep))
                     .Select(e => e.t).Distinct();
                // filtering by a subtask comments
                var tasksQuery2 = tasksQuery.Where(e => statuses.Contains(e.Status))
                       .Join(subtasksQuery, t => t.EntityId, s => s.TaskId, (t, s) => new { t, s }).DefaultIfEmpty()
                       .Join(commentsQuery.Where(c => c.SubTaskId != null), t => t.s.EntityId, c => c.SubTaskId, (e, c) => new { e.t, c }).DefaultIfEmpty()
                       .Where(e => e.t != null && (e.c == null || e.c.Date >= historyDeep))
                       .Select(e => e.t).Distinct();
                // union
                tasksQuery = tasksQuery1.Union(tasksQuery2).Distinct();
            }

            var projects1 = Mapper.Map<List<ProjectDto>>(projectsQuery);

            var tasks1 = Mapper.Map<List<Task1Dto>>(tasksQuery.OrderBy(e => e.Index));
            var taskIds = tasks1.Select(e => e.EntityId).ToList();

            var subtasks1 = Mapper.Map<List<SubTaskDto>>(subtasksQuery.Where(e => taskIds.Contains(e.TaskId)).OrderBy(e => e.Order));
            var subtaskIds = subtasks1.Select(e => e.EntityId).ToList();

            var taskuserList = taskuserRep.SearchFor(e => taskIds.Contains(e.TaskId)).ToList();
            commentsQuery = commentsQuery.Where(e =>
                (e.TaskId != null && taskIds.Contains(e.TaskId.Value)) ||
                (e.SubTaskId != null && subtaskIds.Contains(e.SubTaskId.Value)))
                .OrderBy(e => e.Date)
                .ThenBy(e => e.CreatedDate);

            var comments1 = Mapper.Map<List<CommentDto>>(commentsQuery);

            subtasks1.ForEach(st => st.Comments.AddRange(comments1.Where(c => c.SubTaskId == st.EntityId)));
            tasks1.ForEach(t => t.UserIds.AddRange(taskuserList.Where(c => c.TaskId == t.EntityId).Select(e => e.UserId)));
            tasks1.ForEach(t => t.Comments.AddRange(comments1.Where(c => c.TaskId == t.EntityId)));
            tasks1.ForEach(t => t.SubTasks.AddRange(subtasks1.Where(st => st.TaskId == t.EntityId)));

            projects = projects1;
            tasks = tasks1;
        }

        #endregion [ data ]

        #region [ task ]

        /// <summary>
        ///     Creates or Updates task </summary>
        /// <param name="taskDto">task DTO</param>
        /// <param name="userDto">user who updates the task</param>
        public void SaveTask(Task1Dto taskDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Task1>();
            var model = rep.GetById(taskDto.EntityId);
            if (model == null) {
                taskDto.CompanyId = userDto.CompanyId;
                taskDto.Index = (rep.SearchFor(e => e.CompanyId == userDto.CompanyId).Max(e => (int?)e.Index) ?? 0) + 1;
                model = this.Mapper.Map<Task1>(taskDto);
                rep.Insert(model, userDto.Id);
            } else {
                taskDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(taskDto, model);
                rep.Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();

            this.SaveTaskUsers(taskDto, userDto);

            this.CalculateTaskValues(model.EntityId, userDto);
        }

        /// <summary>
        ///     Synchronizes responsibles </summary>
        /// <param name="taskDto">task DTO</param>
        /// <param name="userDto">user who updates the task</param>
        private void SaveTaskUsers(Task1Dto taskDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<TaskUser>();
            var models = rep.SearchFor(e => e.TaskId == taskDto.EntityId).ToList();
            foreach (var model in models) {
                if (!taskDto.UserIds.Contains(model.UserId)) {
                    rep.DeleteById(model.EntityId);
                }
            }
            foreach (var userId in taskDto.UserIds) {
                if (models.Any(e => e.UserId == userId) == false) {
                    rep.Insert(new TaskUser() { TaskId = taskDto.EntityId, UserId = userId }, userDto.Id);
                }
            }
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Deletes task by id </summary>
        /// <param name="taskId">task id</param>
        /// <param name="userDto">user who deletes the task</param>
        public void DeleteTask(Guid taskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Task1>();
            var model = rep.GetById(taskId);
            if (model != null && model.CompanyId == userDto.CompanyId) {
                rep.MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
            this.DeleteTaskUsers(taskId, userDto);
        }

        /// <summary>
        ///     Deletes responsibles by id </summary>
        /// <param name="taskId">task id</param>
        /// <param name="userDto">user who deletes responsibles</param>
        private void DeleteTaskUsers(Guid taskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<TaskUser>();
            var models = rep.SearchFor(e => e.TaskId == taskId).ToList();
            foreach (TaskUser model in models) {
                rep.MarkAsDelete(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
        }

        #endregion [ task ]

        #region [ subtask ]

        /// <summary>
        ///     Creates or Updates subtask </summary>
        /// <param name="subtaskDto">subtask DTO</param>
        /// <param name="userDto">user who updates the subtask</param>
        public void SaveSubTask(SubTaskDto subtaskDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<SubTask>();
            var model = rep.GetById(subtaskDto.EntityId);
            if (model == null) {
                subtaskDto.CompanyId = userDto.CompanyId;
                subtaskDto.Order = (rep.SearchFor(e => e.CompanyId == userDto.CompanyId && e.TaskId == subtaskDto.TaskId).Max(e => (int?)e.Order) ?? 0) + 1;
                model = this.Mapper.Map<SubTask>(subtaskDto);
                rep.Insert(model, userDto.Id);
            } else {
                subtaskDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(subtaskDto, model);
                rep.Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();

            this.CalculateTaskValues(model.TaskId, userDto);
        }

        /// <summary>
        ///     Deletes subtask by id </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who deletes the subtask</param>
        public void DeleteSubTask(Guid subtaskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<SubTask>();
            var model = rep.GetById(subtaskId);
            if (model != null && model.CompanyId == userDto.CompanyId) {
                rep.MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();

                this.CalculateTaskValues(model.TaskId, userDto);
            }
        }

        /// <summary>
        ///     Changes a order of a subtask </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who changes the subtask</param>
        /// <returns>Changed subtasks</returns>
        public IEnumerable<SubTaskDto> UpSubTask(Guid subtaskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<SubTask>();
            var subTask = rep.GetById(subtaskId);
            if (subTask != null) {
                var models = rep.SearchFor(e =>
                    e.CompanyId == userDto.CompanyId &&
                    e.TaskId == subTask.TaskId).OrderBy(e => e.Order).ToList();
                // to swap
                for (int i = models.Count - 1; i >=  /* skips first */ 1; i--) {
                    if (models[i].EntityId == subtaskId) {
                        var tmp = models[i];
                        models[i] = models[i - 1];
                        models[i - 1] = tmp;
                        break;
                    }
                }
                // sets order
                for (int i = 0; i < models.Count; i++) {
                    if (models[i].Order != i + 1) {
                        models[i].Order = i + 1;
                        yield return Mapper.Map<SubTaskDto>(models[i]);
                    }
                }
                this.UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        ///     Changes a order of a subtask </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who changes the subtask</param>
        /// <returns>Changed subtasks</returns>
        public IEnumerable<SubTaskDto> DownSubTask(Guid subtaskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<SubTask>();
            var subTask = rep.GetById(subtaskId);
            if (subTask != null) {
                var models = rep.SearchFor(e =>
                e.CompanyId == userDto.CompanyId &&
                e.TaskId == subTask.TaskId).OrderBy(e => e.Order).ToList();
                // to swap
                for (int i = 0; i < models.Count /* skips last */ - 1; i++) {
                    if (models[i].EntityId == subtaskId) {
                        var tmp = models[i];
                        models[i] = models[i + 1];
                        models[i + 1] = tmp;
                        break;
                    }
                }
                // sets order
                for (int i = 0; i < models.Count; i++) {
                    if (models[i].Order != i + 1) {
                        models[i].Order = i + 1;
                        yield return Mapper.Map<SubTaskDto>(models[i]);
                    }
                }
                this.UnitOfWork.SaveChanges();
            }
        }

        #endregion [ subtask ]

        #region [ comments ]

        /// <summary>
        ///     Creates or Updates comment </summary>
        /// <param name="commentDto">comment DTO</param>
        /// <param name="userDto">user who updates the comment</param>
        public void SaveComment(CommentDto commentDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Comment>();
            var model = rep.GetById(commentDto.EntityId);
            if (model == null) {
                commentDto.CompanyId = userDto.CompanyId;
                model = this.Mapper.Map<Comment>(commentDto);
                rep.Insert(model, userDto.Id);
            } else {
                commentDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(commentDto, model);
                rep.Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();

            this.CalculateTaskOrSubTaskValues(model.TaskId, model.SubTaskId, userDto);
        }

        /// <summary>
        ///     Deletes comment by id </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="userDto">user who deletes the comment</param>
        public void DeleteComment(Guid commentId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Comment>();
            var model = rep.GetById(commentId);
            if (model != null && model.CompanyId == userDto.CompanyId) {
                rep.MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();

                this.CalculateTaskOrSubTaskValues(model.TaskId, model.SubTaskId, userDto);
            }
        }

        #endregion [ comments ]

        #region [ calculation ]

        /// <summary>
        ///     Updates task values or subtask+task values </summary>
        /// <param name="taskId">task id</param>
        /// <param name="subTaskId">subtask id</param>
        /// <param name="userDto">user who updates</param>
        private void CalculateTaskOrSubTaskValues(Guid? taskId, Guid? subTaskId, UserDto userDto) {
            if (taskId != null) {
                this.CalculateTaskValues(taskId.Value, userDto);
            }
            if (subTaskId != null) {
                var parentTaskId = this.CalculateSubTaskValues(subTaskId.Value, userDto);
                this.CalculateTaskValues(parentTaskId, userDto);
            }
        }

        /// <summary>
        ///     Updates task values </summary>
        /// <param name="taskId">task id</param>
        /// <param name="userDto">user who updates the task</param>
        private void CalculateTaskValues(Guid taskId, UserDto userDto) {
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var commentRep = UnitOfWork.GetRepository<Comment>();

            var task = taskRep.GetById(taskId);
            var subtasks = subtaskRep.SearchFor(e => e.CompanyId == userDto.CompanyId && e.TaskId == taskId).ToList();
            var subtaskIds = subtasks.Select(e => e.EntityId).ToList();

            var comments = commentRep.SearchFor(e => e.CompanyId == userDto.CompanyId);
            var taskComments = comments.Where(e => e.TaskId == taskId).ToList();
            var subtasksComments = comments.Where(e => subtaskIds.Contains(e.SubTaskId.Value)).ToList();

            // sets task.progress from task.comments
            // a task will add subtasks.progress when task has subtasks. CODE#1
            task.Progress = taskComments
                .Where(e => e.Progress != null)
               .OrderBy(e => e.Date).ThenBy(e => e.CreatedDate)
               .Select(e => e.Progress.Value).DefaultIfEmpty(0).LastOrDefault();
            // simple task
            if (subtasks.Any() == false) {
                // sets task.status from task.comments
                task.ActualWork = new TimeSpan(taskComments.Where(e => e.ActualWorkTicks != null).Select(e => e.ActualWorkTicks.Value).DefaultIfEmpty(0).Sum());
            }
            // task with subtasks
            else {
                // sets task.actualwork from task.comments + subtasks.comments
                task.ActualWork = new TimeSpan(
                    // NOTES: I used two List instead Queryable 'comments'
                    taskComments.Where(e => e.ActualWorkTicks != null).Select(e => e.ActualWorkTicks.Value).DefaultIfEmpty(0).Sum() +
                    subtasksComments.Where(e => e.ActualWorkTicks != null).Select(e => e.ActualWorkTicks.Value).DefaultIfEmpty(0).Sum());
                // sets task.totalwork from subtasks
                task.TotalWork = new TimeSpan(subtasks.Sum(e => e.TotalWorkTicks));
                if (task.TotalWork != TimeSpan.Zero) {
                    // sets task.progress from tasks[progress] and subtasks[progress * subtask.totalwork / task.totalwork] 
                    task.Progress = subtasks.Sum(e => e.Progress * e.TotalWorkTicks / task.TotalWorkTicks) + /* CODE#1 */ task.Progress;
                } else {
                    task.Progress = 0;
                }
                // sets task.status from max of subtasks
                if (subtasks
                    .Where(e => e.Status != (byte)TaskStatusEnum.Failed && 
                                e.Status != (byte)TaskStatusEnum.Rejected)
                    .All(e => e.Status == (byte)TaskStatusEnum.Done)) {
                    task.Status = (byte)TaskStatusEnum.Done;
                } else if (subtasks.Any(e => e.Status == (byte)TaskStatusEnum.InProgress)) {
                    task.Status = (byte)TaskStatusEnum.InProgress;
                } else {
                    task.Status = (byte)TaskStatusEnum.NotStarted;
                }
                task.Status = subtasks.Max(e => e.Status);
            }
            if (taskComments.Any()) {
                // sets task.status from task.last_comment
                task.Status = taskComments.OrderBy(e => e.Date).ThenBy(e => e.CreatedDate).Last().Status;
            }

            taskRep.Update(task, userDto.Id);
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Updates subtask values </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who updates the task</param>
        /// <returns>Parent task id</returns>
        private Guid CalculateSubTaskValues(Guid subtaskId, UserDto userDto) {
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var commentRep = UnitOfWork.GetRepository<Comment>();

            var subtask = subtaskRep.GetById(subtaskId);

            var comments = commentRep.SearchFor(e => e.CompanyId == userDto.CompanyId);
            var subtaskComments = comments.Where(e => e.SubTaskId == subtask.EntityId).ToList();

            // sets subtask.progress from task.comments
            subtask.Progress = subtaskComments
                .Where(e => e.Progress != null)
                .OrderBy(e => e.Date).ThenBy(e => e.CreatedDate)
                .Select(e => e.Progress.Value).DefaultIfEmpty(0).LastOrDefault();
            // sets subtask.actualwork from subtask.comments
            subtask.ActualWork = new TimeSpan(subtaskComments.Where(e => e.ActualWorkTicks != null).Select(e => e.ActualWorkTicks.Value).DefaultIfEmpty(0).Sum());

            if (subtaskComments.Any()) {
                // sets subtask.status from subtask.last_comment
                subtask.Status = subtaskComments.OrderBy(e => e.Date).ThenBy(e => e.CreatedDate).Last().Status;
            }

            subtaskRep.Update(subtask, userDto.Id);
            this.UnitOfWork.SaveChanges();

            return subtask.TaskId;
        }

        #endregion [ calculation ]
    }
}
