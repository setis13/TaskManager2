using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data;
using TaskManager.Data.Entities;
using TaskManager.Logic.Dtos;
using TaskManager.Logic.Enums;
using Microsoft.AspNet.Identity;
using TaskManager.Data.Identity;

namespace TaskManager.Logic.Services {
    public class TaskService : HostService<ITaskService>, ITaskService {

        private IFileService _fileService => this.ServicesHost.GetService<IFileService>();

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
        /// <param name="reportFilter">tasks for reporting</param>
        /// <param name="projects">out parameter</param>
        /// <param name="tasks">out parameter</param>
        /// <param name="historyFilters">out parameter</param>
        /// <param name="lastResponsibleIds">out last responsible ids</param>
        /// <param name="lastFavorite">out last favorite value</param>
        /// <param name="lastPriority">out last priority value</param>
        public void GetData(UserDto user, DateTime? historyDeep, bool reportFilter,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks,
            out List<DateTime> historyFilters,
            out List<Guid> lastResponsibleIds,
            out bool lastFavorite,
            out byte lastPriority) {

            var projectRep = UnitOfWork.GetRepository<Project>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var taskuserRep = UnitOfWork.GetRepository<TaskUser>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var commentRep = UnitOfWork.GetRepository<Comment>();
            var favoriteRep = UnitOfWork.GetRepository<UserFavorite>();

            var projectsQuery = projectRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var tasksQuery = taskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var subtasksQuery = subtaskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var commentsQuery = commentRep.SearchFor(e => e.CompanyId == user.CompanyId);

            historyFilters = commentsQuery.Select(e => e.Date).ToList()
                .Select(e => e.Date.AddDays(-e.Date.Day + 1)).Distinct().OrderByDescending(e => e).ToList();

            lastResponsibleIds = this.GetLastResponsibleIds(user);
            lastFavorite = this.GetLastFavorite(user);
            lastPriority = this.GetLastPriority(user);

            if (reportFilter == true) {
                var todayStarted = DateTime.Now.AddDays(-1);
                tasksQuery = tasksQuery.Where(e => e.LastModifiedDate >= todayStarted);
                subtasksQuery = subtasksQuery.Where(e => e.LastModifiedDate >= todayStarted);
                commentsQuery = commentsQuery.Where(e => e.LastModifiedDate >= todayStarted);
            } else if (historyDeep == null) {
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
                // filtering by a task
                tasksQuery = tasksQuery.Where(e => statuses.Contains(e.Status) && e.LastModifiedDate >= historyDeep);
            }

            var projects1 = Mapper.Map<List<ProjectDto>>(projectsQuery.OrderByDescending(e => e.Count));

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

            var files = this._fileService.GetModels(
                Enumerable.Union(
                    Enumerable.Union(
                        comments1.Select(e => e.EntityId),
                        subtasks1.Select(e => e.EntityId)),
                    tasks1.Select(e => e.EntityId)
                ).ToList());
            comments1.ForEach(e => e.Files = files.Where(f => f.ParentId == e.EntityId).ToList());
            subtasks1.ForEach(e => e.Files = files.Where(f => f.ParentId == e.EntityId).ToList());
            tasks1.ForEach(e => e.Files = files.Where(f => f.ParentId == e.EntityId).ToList());

            // favorites
            var taskIdsFavorites = favoriteRep.SearchFor(e => e.UserId == user.Id && e.TaskId != null && e.SubTaskId == null && taskIds.Contains(e.TaskId.Value)).Select(e => e.TaskId).ToList();
            var subtaskIdsFavorites = favoriteRep.SearchFor(e => e.UserId == user.Id && e.TaskId != null && e.SubTaskId != null && subtaskIds.Contains(e.SubTaskId.Value)).Select(e => e.SubTaskId).ToList();
            tasks1.Where(e => taskIdsFavorites.Contains(e.EntityId)).ToList().ForEach(t => t.Favorite = true);
            subtasks1.Where(e => subtaskIdsFavorites.Contains(e.EntityId)).ToList().ForEach(t => t.Favorite = true);

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

            this.SetLastModifiedTaskId(taskDto.EntityId, userDto);
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

            this.SetTaskFavorite(taskDto.Favorite, taskDto.EntityId, userDto);
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

            this.SetSubTaskFavorite(subtaskDto.Favorite, subtaskDto.TaskId, subtaskDto.EntityId, userDto);

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
                if (subtasks.Any(e => e.TotalWorkTicks != null)) {
                    task.TotalWork = new TimeSpan(subtasks.Where(e => e.TotalWorkTicks != null && e.Status != (byte)TaskStatusEnum.Rejected).Sum(e => e.TotalWorkTicks.Value));
                    if (task.TotalWork != TimeSpan.Zero) {
                        // sets task.progress from tasks[progress] and subtasks[progress * subtask.totalwork / task.totalwork] 
                        task.Progress = subtasks.Where(e => e.TotalWorkTicks != null && e.Status != (byte)TaskStatusEnum.Rejected).Sum(e => e.Progress * e.TotalWorkTicks.Value / task.TotalWorkTicks.Value) + /* CODE#1 */ task.Progress;
                    } else {
                        task.Progress = 0;
                    }
                } else {
                    task.TotalWork = null;
                    task.Progress = (float)subtasks.Where(e => e.Status == (byte)TaskStatusEnum.Done).Count() / subtasks.Count(e => e.Status != (byte)TaskStatusEnum.Rejected);
                }
                // sets task.status from max of subtasks
                if (subtasks
                    .Where(e => e.Status != (byte)TaskStatusEnum.Failed &&
                                e.Status != (byte)TaskStatusEnum.Rejected)
                    .All(e => e.Status == (byte)TaskStatusEnum.Done)) {
                    task.Status = (byte)TaskStatusEnum.Done;
                } else if (subtasks.Any(e => e.Status == (byte)TaskStatusEnum.InProgress || e.Status == (byte)TaskStatusEnum.Done)) {
                    task.Status = (byte)TaskStatusEnum.InProgress;
                } else {
                    task.Status = (byte)TaskStatusEnum.NotStarted;
                }
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

        #region [ last task ]

        /// <summary>
        ///     Sets last task id </summary>
        private void SetLastModifiedTaskId(Guid taskId, UserDto userDto) {
            if (userDto.LastModifiedTaskId != taskId) {
                TaskManagerUser user = base.ServicesHost.UserManager.FindById(userDto.Id);
                user.LastModifiedTaskId = taskId;
                base.ServicesHost.UserManager.Update(user);
            }
        }

        /// <summary>
        ///     Gets last favorite </summary>
        private bool GetLastFavorite(UserDto userDto) {
            if (userDto.LastModifiedTaskId != null) {
                var taskRep = UnitOfWork.GetRepository<UserFavorite>();
                var any = taskRep.SearchFor(e => e.UserId == userDto.Id && e.TaskId == userDto.LastModifiedTaskId.Value).Any();
                return any;
            }
            return false;
        }

        /// <summary>
        ///     Gets last priority </summary>
        private byte GetLastPriority(UserDto userDto) {
            if (userDto.LastModifiedTaskId != null) {
                var taskRep = UnitOfWork.GetRepository<Task1>();
                var priority = taskRep.SearchFor(e => e.CreatedById == userDto.Id && e.EntityId == userDto.LastModifiedTaskId.Value)
                    .Select(e => e.Priority).DefaultIfEmpty((byte)0).FirstOrDefault();
                return priority;
            }
            return 0;
        }

        /// <summary>
        ///     Gets last responsible </summary>
        private List<Guid> GetLastResponsibleIds(UserDto userDto) {
            if (userDto.LastModifiedTaskId != null) {
                var userTaskRep = UnitOfWork.GetRepository<TaskUser>();
                var userIds = userTaskRep
                    .SearchFor(e => (e.CreatedById == userDto.Id || e.LastModifiedById == userDto.Id) &&
                                    e.TaskId == userDto.LastModifiedTaskId.Value)
                    .Select(e => e.UserId).ToList();
                return userIds;
            }
            return null;
        }

        #endregion [ last task ]

        #region [ Favorites ]

        /// <summary>
        ///     Creates/Deletes entity of favorite for a task </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        private void SetTaskFavorite(bool favorite, Guid taskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<UserFavorite>();
            var modelIds = rep.SearchFor(e => e.UserId == userDto.Id && e.TaskId == taskId).Select(e => e.EntityId).ToList();
            if (modelIds.Any() && favorite == false) {
                rep.DeleteByIds(modelIds);
                this.UnitOfWork.SaveChanges();
            } else if (modelIds.Any() == false && favorite == true) {
                var model = new UserFavorite();
                model.UserId = userDto.Id;
                model.TaskId = taskId;
                rep.Insert(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        ///     Creates/Deletes entity of favorite for a subtask </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="subtaskId">SubTask ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        private void SetSubTaskFavorite(bool favorite, Guid taskId, Guid subtaskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<UserFavorite>();
            var model = rep.SearchFor(e => e.UserId == userDto.Id && e.SubTaskId == subtaskId).FirstOrDefault();
            if (model != null && favorite == false) {
                rep.DeleteById(model.EntityId);
                this.UnitOfWork.SaveChanges();
            } else if (model == null && favorite == true) {
                this.SetTaskFavorite(true, taskId, userDto);
                model = new UserFavorite();
                model.UserId = userDto.Id;
                model.TaskId = taskId;
                model.SubTaskId = subtaskId;
                rep.Insert(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        ///     Creates/Deletes entity of favorite for a task </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        public bool InvertTaskFavorite(Guid taskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<UserFavorite>();
            var modelIds = rep.SearchFor(e => e.UserId == userDto.Id && e.TaskId == taskId).Select(e => e.EntityId).ToList();
            if (modelIds.Any()) {
                rep.DeleteByIds(modelIds);
                this.UnitOfWork.SaveChanges();
                return false;
            } else {
                var model = new UserFavorite();
                model.UserId = userDto.Id;
                model.TaskId = taskId;
                rep.Insert(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
                return true;
            }
        }

        /// <summary>
        ///     Creates/Deletes entity of favorite for a subtask </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="subtaskId">SubTask ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        public bool InvertSubTaskFavorite(Guid taskId, Guid subtaskId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<UserFavorite>();
            var model = rep.SearchFor(e => e.UserId == userDto.Id && e.SubTaskId == subtaskId).FirstOrDefault();
            if (model != null) {
                rep.DeleteById(model.EntityId);
                this.UnitOfWork.SaveChanges();
                return false;
            } else {
                this.SetTaskFavorite(true, taskId, userDto);
                model = new UserFavorite();
                model.UserId = userDto.Id;
                model.TaskId = taskId;
                model.SubTaskId = subtaskId;
                rep.Insert(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
                return true;
            }
        }

        #endregion [ Favorites ]
    }
}
