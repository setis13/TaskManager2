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
        /// <param name="subtasks">out parameter</param>
        /// <param name="comments">out parameter</param>
        public void GetData(UserDto user, DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks,
            out List<SubTaskDto> subtasks,
            out List<CommentDto> comments) {

            var projectRep = UnitOfWork.GetRepository<Project>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
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
            commentsQuery = commentsQuery.Where(e => 
                (e.TaskId != null && taskIds.Contains(e.TaskId.Value)) ||
                (e.SubTaskId != null && subtaskIds.Contains(e.SubTaskId.Value)));

            var comments1 = Mapper.Map<List<CommentDto>>(commentsQuery);

            subtasks1.ForEach(st => st.Comments = comments1.Where(c => c.SubTaskId == st.EntityId).ToList());
            tasks1.ForEach(t => t.Comments = comments1.Where(c => c.TaskId == t.EntityId).ToList());
            tasks1.ForEach(t => t.SubTasks = subtasks1.Where(st => st.TaskId == t.EntityId).ToList());

            projects = projects1;
            tasks = tasks1;
            subtasks = subtasks1;
            comments = comments1;
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
                model = this.Mapper.Map<Task1>(taskDto);
                this.UnitOfWork.GetRepository<Task1>().Insert(model, userDto.Id);
            } else {
                taskDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(taskDto, model);
                this.UnitOfWork.GetRepository<Task1>().Update(model, userDto.Id);
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
                this.UnitOfWork.GetRepository<Task1>().MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
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
                model = this.Mapper.Map<SubTask>(subtaskDto);
                this.UnitOfWork.GetRepository<SubTask>().Insert(model, userDto.Id);
            } else {
                subtaskDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(subtaskDto, model);
                this.UnitOfWork.GetRepository<SubTask>().Update(model, userDto.Id);
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
                this.UnitOfWork.GetRepository<SubTask>().MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
