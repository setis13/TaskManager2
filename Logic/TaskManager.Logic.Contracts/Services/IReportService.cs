using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Report Service interface. </summary>
    public interface IReportService : IService {

        /// <summary>
        ///     Gets data for report by day </summary>
        /// <param name="date">Day</param>
        /// <param name="user">User DTO</param>
        /// <returns>List of Project DTOs</returns>
        List<ReportProjectDto> GetSingleDayData(DateTime date, UserDto user);
    }
}