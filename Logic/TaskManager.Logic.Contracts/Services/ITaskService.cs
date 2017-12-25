using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Task Service interface. </summary>
    public interface ITaskService : IService {
        /// <summary>
        ///     Returns tasks </summary>
        /// <remarks> if <see cref="historyDeep"/> is null then returns actual tasks else actual + history </remarks>
        /// <param name="historyDeep">minimum date of tasks</param>
        /// <param name="projects">List of projects. Returns deleted projects when use <see cref="historyDeep"/></param>
        /// <param name="tasks">List of tasks</param>
        /// <param name="subTasks">List of subtasks of tasks</param>
        /// <param name="comments">List of comments of tasks or subtasks</param>
        void GetData(DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks, 
            out List<SubTaskDto> subTasks, 
            out List<CommentDto> comments);
    }
}