using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Enums;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class MockTaskService : HostService<ITaskService>, ITaskService {
        public MockTaskService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) : base(servicesHost, unitOfWork, mapper) {
        }

        public void GetData(DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks,
            out List<SubTaskDto> subTasks,
            out List<CommentDto> comments) {

            #region projects

            var project1 = new ProjectDto() {
                Name = "Project 1"
            };
            var project2 = new ProjectDto() {
                Name = "Project 2"
            };
            var project3 = new ProjectDto() {
                Name = "Project 3",
            };

            #endregion projects

            #region tasks

            var index = 0;

            var task1_1 = new Task1Dto() {
                ProjectId = project1.EntityId,
                Index = ++index,
                Name = "Task 1",
                Description = "description",
                TotalWork = TimeSpan.FromHours(40),
                Priority = (byte)TaskPriorityEnum.Medium,
                Status = (byte)TaskStatusEnum.Created,
            };
            var task1_2 = new Task1Dto() {
                ProjectId = project1.EntityId,
                Index = ++index,
                Name = "Task 2",
                TotalWork = TimeSpan.FromHours(10.5),
                Priority = (byte)TaskPriorityEnum.Low,
                Status = (byte)TaskStatusEnum.InProgress,
            };
            var task1_3 = new Task1Dto() {
                ProjectId = project1.EntityId,
                Index = ++index,
                Name = "Task 3",
                TotalWork = TimeSpan.FromHours(10),
                Priority = (byte)TaskPriorityEnum.Low,
                Status = (byte)TaskStatusEnum.Failed,
            };

            var task2_1 = new Task1Dto() {
                ProjectId = project1.EntityId,
                Index = ++index,
                Name = "Task 1",
                TotalWork = TimeSpan.FromMinutes(40),
                Priority = (byte)TaskPriorityEnum.High,
                Status = (byte)TaskStatusEnum.InProgress,
            };
            var task2_2 = new Task1Dto() {
                ProjectId = project2.EntityId,
                Index = ++index,
                Name = "Task 2",
                TotalWork = TimeSpan.FromHours(10),
                Priority = (byte)TaskPriorityEnum.Medium,
                Status = (byte)TaskStatusEnum.Done,
            };
            var task2_3 = new Task1Dto() {
                ProjectId = project2.EntityId,
                Index = ++index,
                Name = "Task 3",
                TotalWork = TimeSpan.FromHours(10),
                Priority = (byte)TaskPriorityEnum.Low,
                Status = (byte)TaskStatusEnum.InProgress,
            };
            var task2_4 = new Task1Dto() {
                ProjectId = project2.EntityId,
                Index = ++index,
                Name = "Task 4",
                TotalWork = TimeSpan.FromHours(10),
                Priority = (byte)TaskPriorityEnum.Medium,
                Status = (byte)TaskStatusEnum.Rejected,
            };

            var task3_1 = new Task1Dto() {
                ProjectId = project3.EntityId,
                Index = ++index,
                Name = "Task 1",
                TotalWork = TimeSpan.FromMinutes(40),
                Priority = (byte)TaskPriorityEnum.Low,
                Status = (byte)TaskStatusEnum.Created,
            };

            #endregion tasks

            #region subtasks

            var subtask1_1 = new SubTaskDto() {
                TaskId = task1_1.EntityId,
                Name = "SubTask 1",
                TotalWork = TimeSpan.FromHours(8),
                Status = (byte)TaskStatusEnum.Created,
            };
            var subtask1_2 = new SubTaskDto() {
                TaskId = task1_1.EntityId,
                Name = "SubTask 2",
                TotalWork = TimeSpan.FromHours(4),
                Status = (byte)TaskStatusEnum.InProgress,
            };
            var subtask1_3 = new SubTaskDto() {
                TaskId = task1_1.EntityId,
                Name = "SubTask 3",
                TotalWork = TimeSpan.FromHours(4),
                Status = (byte)TaskStatusEnum.Failed,
            };

            var subtask2_1 = new SubTaskDto() {
                TaskId = task2_1.EntityId,
                Name = "SubTask 1",
                TotalWork = TimeSpan.FromMinutes(40),
                Status = (byte)TaskStatusEnum.InProgress,
            };
            var subtask2_2 = new SubTaskDto() {
                TaskId = task2_1.EntityId,
                Name = "SubTask 2",
                TotalWork = TimeSpan.FromHours(10),
                Status = (byte)TaskStatusEnum.Done,
            };
            var subtask2_3 = new SubTaskDto() {
                TaskId = task2_1.EntityId,
                Name = "SubTask 3",
                TotalWork = TimeSpan.FromHours(10),
                Status = (byte)TaskStatusEnum.InProgress,
            };

            #endregion subtasks

            #region comments

            var comment1_1 = new CommentDto() {
                SubTaskId = subtask2_1.EntityId,
                Description = "Commenct 1",
                ActualWork = TimeSpan.FromHours(4),
            };
            var comment1_2 = new CommentDto() {
                SubTaskId = subtask2_1.EntityId,
                Description = "Commenct 2",
                ActualWork = TimeSpan.FromHours(2),
            };

            var comment2_1 = new CommentDto() {
                TaskId = task1_2.EntityId,
                Description = "Commenct 1",
                ActualWork = TimeSpan.FromHours(4),
            };
            var comment2_2 = new CommentDto() {
                TaskId = task1_2.EntityId,
                Description = "Commenct 2",
                ActualWork = TimeSpan.FromHours(2),
            };
            #endregion comments

            projects = new List<ProjectDto>() { project1, project2, project3 };
            var alltasks = new List<Task1Dto>() { task1_1, task1_2, task1_3, task2_1, task2_2, task2_3, task2_4, task3_1 };
            var allsubtasks = new List<SubTaskDto>() { subtask1_1, subtask1_2, subtask1_3, subtask2_1, subtask2_2, subtask2_3 };
            var allcomments = new List<CommentDto> { comment1_1, comment1_2, comment2_1, comment2_2 };
            if (historyDeep == null) {
                var statuses = new List<TaskStatusEnum>() { TaskStatusEnum.Created, TaskStatusEnum.InProgress };
                tasks = alltasks.Where(e => statuses.Contains((TaskStatusEnum)e.Status)).ToList();
                subTasks = allsubtasks.Where(e => statuses.Contains((TaskStatusEnum) e.Status)).ToList();
                var taskIds = tasks.Select(e => e.EntityId).ToList();
                var subtaskIds = subTasks.Select(e => e.EntityId).ToList();
                comments = allcomments.Where(e => taskIds.Contains(e.TaskId) || subtaskIds.Contains(e.SubTaskId)).ToList();
            } else {
                tasks = alltasks;
                subTasks = allsubtasks;
                comments = allcomments;
            }
        }
    }
}
