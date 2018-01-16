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
        public TaskService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Gets projects, tasks, subtasks, comments by user </summary>
        /// <param name="user">user who requests the data</param>
        /// <remarks> if <see cref="historyDeep"/> is null then returns actual tasks else actual + history </remarks>
        /// <param name="historyDeep">minimum date of tasks</param>
        /// <param name="projects">out parameter</param>
        /// <param name="tasks">out parameter</param>
        public void GetData(UserDto user, DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks) {

            var projectRep = UnitOfWork.GetRepository<Project>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var taskuserRep = UnitOfWork.GetRepository<TaskUser>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var commentRep = UnitOfWork.GetRepository<Comment>();

            var projectsQuery = projectRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var tasksQuery = taskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var subtasksQuery = subtaskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var commentsQuery = commentRep.SearchFor(e => e.CompanyId == user.CompanyId);

            if (historyDeep == null) {
                var statuses = new List<TaskStatusEnum>() {
                    TaskStatusEnum.NotStarted,
                    TaskStatusEnum.InProgress
                }.Cast<int>().ToList();
                tasksQuery = tasksQuery.Where(e => statuses.Contains(e.Status));
                subtasksQuery = subtasksQuery.Where(e => statuses.Contains(e.Status));
            } else {
                tasksQuery = tasksQuery.Where(e => e.CreatedDate >= historyDeep);
                subtasksQuery = subtasksQuery.Where(e => e.CreatedDate >= historyDeep);
            }

            var projects1 = Mapper.Map<List<ProjectDto>>(projectsQuery);
            var tasks1 = Mapper.Map<List<Task1Dto>>(tasksQuery);
            var subtasks1 = Mapper.Map<List<SubTaskDto>>(subtasksQuery);

            var taskIds = tasks1.Select(e => e.EntityId).ToList();
            var subtaskIds = subtasks1.Select(e => e.EntityId).ToList();

            var taskuserList = taskuserRep.SearchFor(e => taskIds.Contains(e.TaskId)).ToList();
            commentsQuery = commentsQuery.Where(e =>
                (e.TaskId != null && taskIds.Contains(e.TaskId.Value)) ||
                (e.SubTaskId != null && subtaskIds.Contains(e.SubTaskId.Value)));

            var comments1 = Mapper.Map<List<CommentDto>>(commentsQuery);

            subtasks1.ForEach(st => st.Comments = comments1.Where(c => c.SubTaskId == st.EntityId).ToList());
            tasks1.ForEach(t => t.UserIds = taskuserList.Where(c => c.TaskId == t.EntityId).Select(e => e.UserId).ToList());
            tasks1.ForEach(t => t.Comments = comments1.Where(c => c.TaskId == t.EntityId).ToList());
            tasks1.ForEach(t => t.SubTasks = subtasks1.Where(st => st.TaskId == t.EntityId).OrderBy(e => e.Order).ToList());

            projects = projects1;
            tasks = tasks1;
        }

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
            DeleteTaskUsers(taskId, userDto);
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

        /// <summary>
        ///     Creates or Updates subtask </summary>
        /// <param name="subtaskDto">subtask DTO</param>
        /// <param name="userDto">user who updates the subtask</param>
        public void SaveSubTask(SubTaskDto subtaskDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<SubTask>();
            var model = rep.GetById(subtaskDto.EntityId);
            if (model == null) {
                subtaskDto.CompanyId = userDto.CompanyId;
                subtaskDto.Order = (rep.SearchFor(e => e.CompanyId == userDto.CompanyId).Max(e => (int?)e.Order) ?? 0) + 1;
                model = this.Mapper.Map<SubTask>(subtaskDto);
                rep.Insert(model, userDto.Id);
            } else {
                subtaskDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(subtaskDto, model);
                rep.Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
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
                UnitOfWork.SaveChanges();
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
                UnitOfWork.SaveChanges();
            }
        }

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
            }
        }
    }
}
