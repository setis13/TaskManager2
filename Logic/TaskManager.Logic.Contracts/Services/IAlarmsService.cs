using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Alarms Service interface. </summary>
    public interface IAlarmsService : IService {
        /// <summary>
        ///     Returns alarms </summary>
        /// <param name="userDto">user who requests the alarms</param>
        /// <returns>List of alarms</returns>
        List<AlarmDto> GetData(UserDto userDto);
        /// <summary>
        ///     Returns alarms for menu's tooltip </summary>
        /// <param name="date">date today</param>
        /// <param name="userDto">user who requests the alarms</param>
        /// <returns>List of alarms</returns>
        List<AlarmDto> GetNearAlarms(DateTime date, UserDto userDto);
        /// <summary>
        ///     Creates or Updates alarm </summary>
        /// <param name="alarmDto">alarm DTO</param>
        /// <param name="userDto">user who updates the alarm</param>
        void Save(AlarmDto alarmDto, UserDto userDto);
        /// <summary>
        ///     Deletes alarm by id </summary>
        /// <param name="id">alarm id</param>
        /// <param name="userDto">user who deletes the alarm</param>
        void Delete(Guid alarmId, UserDto userDto);
    }
}