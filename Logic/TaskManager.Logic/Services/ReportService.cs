﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data;
using TaskManager.Data.Entities;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    public class ReportService : HostService, IReportService {
        public ReportService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Gets data for report by day </summary>
        /// <param name="start">Start Date and Time</param>
        /// <param name="end">End Date and Time</param>
        /// <param name="includeNew">to include new tasks without comments</param>
        /// <param name="includeZero">to include 0 hour tasks</param>
        /// <param name="projectIds">a filter by projects</param>
        /// <param name="user">User DTO</param>
        /// <returns>List of Project DTOs</returns>
        public List<ReportProjectDto> GetData(
            DateTime start,
            DateTime end,
            bool includeNew,
            bool includeZero,
            List<Guid> projectIds,
            UserDto user) {
            var projectDtos = new List<ReportProjectDto>();
            var taskDtos = new List<ReportTaskDto>();
            var subtaskDtos = new List<ReportSubTaskDto>();
            var commentTaskDtos = new List<ReportCommentDto>();
            var commentSubTaskDtos = new List<ReportCommentDto>();

            var commentRep = UnitOfWork.GetRepository<Comment>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var projectRep = UnitOfWork.GetRepository<Project>();

            var projectsQuery = projectRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var tasksQuery = taskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var subtasksQuery = subtaskRep.SearchFor(e => e.CompanyId == user.CompanyId);
            var commentsQuery = commentRep.SearchFor(e => e.CompanyId == user.CompanyId);

            #region [ commentTaskDtos ]

            // calcs total comments by taskId
            var totalComments1 = commentsQuery
                .Where(e =>
                    e.CreatedById == user.Id &&
                    e.TaskId != null &&
                    start <= e.Date && e.Date <= end)
                .GroupBy(e => e.TaskId)
                .Select(group => new { TaskId = group.Key, Total = group.Count() }).ToList();
            // loop by taskId
            foreach (var totalComment in totalComments1) {
                var total = totalComment.Total;
                // gets total + 1.
                // extra comment is needed to calc delta % for first comment
                var comments = commentsQuery.Where(e =>
                        e.CreatedById == user.Id &&
                        e.TaskId == totalComment.TaskId &&
                        (includeZero || (e.ActualWorkTicks != null && e.ActualWorkTicks != 0)) &&
                        e.Date <= end /* && start <= e.Date // NOTE: to take an old comment */
                    ).OrderByDescending(e => e.Date).ThenBy(e => e.CreatedDate).Take(total + 1)
                    /* Replaces desc sorting to asc */
                    .OrderBy(e => e.Date).ToList();
                for (int i = 0; i < comments.Count; i++) {
                    // checks as before in linq
                    if (start <= comments[i].Date && comments[i].Date <= end) {
                        var dto = Mapper.Map<ReportCommentDto>(comments[i]);
                        if (i - 1 >= 0) {
                            dto.DeltaProgress = comments[i].Progress - comments[i - 1].Progress;
                        } else {
                            dto.DeltaProgress = comments[i].Progress;
                        }
                        commentTaskDtos.Add(dto);
                    }
                }
            }

            #endregion [ commentTaskDtos ]

            #region [ commentSubTaskDtos ]

            // calcs total comments by taskId
            var totalComments2 = commentsQuery
                .Where(e =>
                    e.CreatedById == user.Id &&
                    e.SubTaskId != null &&
                    start <= e.Date && e.Date <= end)
                .GroupBy(e => e.SubTaskId)
                .Select(group => new { SubTaskId = group.Key, Total = group.Count() }).ToList();
            // loop by taskId
            foreach (var totalComment in totalComments2) {
                var total = totalComment.Total;
                // gets total + 1.
                // extra comment is needed to calc delta % for first comment
                var comments = commentsQuery.Where(e =>
                        e.CreatedById == user.Id &&
                        e.SubTaskId == totalComment.SubTaskId &&
                        (includeZero || (e.ActualWorkTicks != null && e.ActualWorkTicks != 0)) &&
                        e.Date <= end /* && start <= e.Date // NOTE: to take an old comment */
                    ).OrderByDescending(e => e.Date).ThenBy(e => e.CreatedDate).Take(total + 1)
                    /* Replaces desc sorting to asc */
                    .OrderBy(e => e.Date).ToList();
                for (int i = 0; i < comments.Count; i++) {
                    // checks as before in linq
                    if (start <= comments[i].Date && comments[i].Date <= end) {
                        var dto = Mapper.Map<ReportCommentDto>(comments[i]);
                        if (i - 1 >= 0) {
                            dto.DeltaProgress = comments[i].Progress - comments[i - 1].Progress;
                        } else {
                            dto.DeltaProgress = comments[i].Progress;
                        }
                        commentSubTaskDtos.Add(dto);
                    }
                }
            }

            #endregion [ commentSubTaskDtos ]

            // gets subtasks
            var subtaskIds = totalComments2.Select(e => e.SubTaskId).ToList();
            List<SubTask> subtasks;
            if (includeNew) {
                subtasks = subtaskRep.SearchFor(e => subtaskIds.Contains(e.EntityId) || start <= e.CreatedDate && e.CreatedDate <= end).ToList();
            } else {
                subtasks = subtaskRep.SearchFor(e => subtaskIds.Contains(e.EntityId)).ToList();
            }
            subtaskDtos = Mapper.Map<List<ReportSubTaskDto>>(subtasks);
            // gets tasks
            var taskIds = totalComments1.Select(e => e.TaskId.Value).Union(subtasks.Select(e => e.TaskId)).ToList();
            List<Task1> tasks;
            if (includeNew) {
                tasks = taskRep.SearchFor(e => taskIds.Contains(e.EntityId) || start <= e.CreatedDate && e.CreatedDate <= end).ToList();
            } else {
                tasks = taskRep.SearchFor(e => taskIds.Contains(e.EntityId)).ToList();
            }
            taskDtos = Mapper.Map<List<ReportTaskDto>>(tasks);
            // gets projects
            var projectIds2 = tasks.Select(e => e.ProjectId).ToList();
            var projects = projectRep.SearchFor(e => projectIds2.Contains(e.EntityId)).ToList();
            projectDtos = Mapper.Map<List<ReportProjectDto>>(projects);

            // NOTES: we have all prepared collections.
            // To prepare result

            foreach (var commentDto in commentSubTaskDtos) {
                var subtaskDto = subtaskDtos.FirstOrDefault(e => e.EntityId == commentDto.SubTaskId.Value);
                // if a subtask was deleted
                if (subtaskDto == null) {
                    subtaskDto = ReportSubTaskDto.CreateById(commentDto.SubTaskId.Value);
                    subtaskDtos.Add(subtaskDto);
                }
                // filters by 0
                if (commentDto.ActualWork != null && commentDto.ActualWork != TimeSpan.Zero) {
                    subtaskDto.ReportComments.Add(commentDto);
                }
            }
            foreach (var commentDto in commentTaskDtos) {
                var taskDto = taskDtos.FirstOrDefault(e => e.EntityId == commentDto.TaskId.Value);
                // if a task was deleted
                if (taskDto == null) {
                    taskDto = ReportTaskDto.CreateById(commentDto.TaskId.Value);
                    taskDtos.Add(taskDto);
                }
                // filters by 0
                if (commentDto.ActualWork != null && commentDto.ActualWork != TimeSpan.Zero) {
                    taskDto.ReportComments.Add(commentDto);
                }
            }
            foreach (var subtaskDto in subtaskDtos.OrderBy(e => e.Order)) {
                var taskDto = taskDtos.FirstOrDefault(e => e.EntityId == subtaskDto.TaskId);
                // if a task was deleted
                if (taskDto == null) {
                    taskDto = ReportTaskDto.CreateById(subtaskDto.TaskId);
                    taskDtos.Add(taskDto);
                }
                // filters by 0
                if (subtaskDto.SumActualWork != TimeSpan.Zero) {
                    taskDto.ReportSubTasks.Add(subtaskDto);
                }
            }
            foreach (var taskDto in taskDtos.OrderByDescending(e => e.Index)) {
                var projectDto = projectDtos.FirstOrDefault(e => e.EntityId == taskDto.ProjectId);
                // if a project was deleted
                if (projectDto == null) {
                    projectDto = ReportProjectDto.CreateById(taskDto.ProjectId);
                    projectDtos.Add(projectDto);
                }
                // filters by 0
                if (taskDto.SumActualWork != TimeSpan.Zero) {
                    projectDto.ReportTasks.Add(taskDto);
                }
            }
            // to filter by projects
            if (projectIds != null && projectIds.Any()) {
                projectDtos = projectDtos.Where(e => projectIds.Contains(e.EntityId)).ToList();
            }
            // filters by 0
            projectDtos = projectDtos.Where(e => e.SumActualWork != TimeSpan.Zero).ToList();
            return projectDtos;
        }
    }
}
