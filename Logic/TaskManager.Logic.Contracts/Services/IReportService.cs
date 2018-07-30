using System;
using System.Collections.Generic;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    /// <summary>
    ///     The Report Service interface. </summary>
    public interface IReportService : IService {

        /// <summary>
        ///     Gets data for report by day </summary>
        /// <param name="start">Start Date and Time</param>
        /// <param name="end">End Date and Time</param>
        /// <param name="includeNew">to include new tasks without comments</param>
        /// <param name="projectIds">a filter by projects</param>
        /// <param name="user">User DTO</param>
        /// <returns>List of Project DTOs</returns>
        List<ReportProjectDto> GetData(DateTime start, DateTime end, bool includeNew, List<Guid> projectIds, UserDto user);
    }
}