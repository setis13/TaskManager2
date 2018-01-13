﻿using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Task Service interface. </summary>
    public interface ITaskService : IService {

        /// <summary>
        ///     Gets projects, tasks, subtasks, comments by user </summary>
        /// <param name="user">user who requests the data</param>
        /// <remarks> if <see cref="historyDeep"/> is null then returns actual tasks else actual + history </remarks>
        /// <param name="historyDeep">minimum date of tasks</param>
        /// <param name="projects">out parameter</param>
        /// <param name="tasks">out parameter</param>
        void GetData(UserDto user, DateTime? historyDeep,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks);

        /// <summary>
        ///     Creates or Updates task </summary>
        /// <param name="taskDto">task DTO</param>
        /// <param name="userDto">user who updates the task</param>
        void SaveTask(Task1Dto taskDto, UserDto userDto);

        /// <summary>
        ///     Deletes task by id </summary>
        /// <param name="taskId">task id</param>
        /// <param name="userDto">user who deletes the task</param>
        void DeleteTask(Guid taskId, UserDto userDto);

        /// <summary>
        ///     Creates or Updates subtask </summary>
        /// <param name="subtaskDto">subtask DTO</param>
        /// <param name="userDto">user who updates the subtask</param>
        void SaveSubTask(SubTaskDto subtaskDto, UserDto userDto);

        /// <summary>
        ///     Deletes subtask by id </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who deletes the subtask</param>
        void DeleteSubTask(Guid subtaskId, UserDto userDto);

        /// <summary>
        ///     Changes a order of a subtask </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who changes the subtask</param>
        /// <returns>Changed subtasks</returns>
        IEnumerable<SubTaskDto> UpSubTask(Guid subtaskId, UserDto userDto);

        /// <summary>
        ///     Changes a order of a subtask </summary>
        /// <param name="subtaskId">subtask id</param>
        /// <param name="userDto">user who changes the subtask</param>
        /// <returns>Changed subtasks</returns>
        IEnumerable<SubTaskDto> DownSubTask(Guid subtaskId, UserDto userDto);
    }
}