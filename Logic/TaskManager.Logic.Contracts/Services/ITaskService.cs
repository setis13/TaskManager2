using System;
using System.Collections.Generic;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    /// <summary>
    ///     The Task Service interface. </summary>
    public interface ITaskService : IService {

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
        void GetData(UserDto user, DateTime? historyDeep, bool reportFilter,
            out List<ProjectDto> projects,
            out List<Task1Dto> tasks,
            out List<DateTime> historyFilters,
            out List<Guid> lastResponsibleIds,
            out bool lastFavorite,
            out byte lastPriority);

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

        /// <summary>
        ///     Creates or Updates comment </summary>
        /// <param name="commentDto">comment DTO</param>
        /// <param name="userDto">user who updates the comment</param>
        void SaveComment(CommentDto commentDto, UserDto userDto);

        /// <summary>
        ///     Deletes comment by id </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="userDto">user who deletes the comment</param>
        void DeleteComment(Guid commentId, UserDto userDto);

        /// <summary>
        ///     Creates/Deletes entity of favorite for a task </summary>
        /// <param name="taskId">Task ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        bool InvertTaskFavorite(Guid taskId, UserDto userDto);

        /// <summary>
        ///     Creates/Deletes entity of favorite for a subtask </summary>
        /// <param name="subtaskId">Task ID</param>
        /// <param name="subtaskId">SubTask ID</param>
        /// <param name="userDto">user DTO</param>
        /// <returns>Value of flag</returns>
        bool InvertSubTaskFavorite(Guid taskId, Guid subtaskId, UserDto userDto);
    }
}