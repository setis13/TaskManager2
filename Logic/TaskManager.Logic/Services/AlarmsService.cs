using System;
using System.Collections.Generic;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;
using System.Linq;
using TaskManager.Logic.Contracts.Enums;

namespace TaskManager.Logic.Services {
    public class AlarmsService : HostService<IAlarmsService>, IAlarmsService {

        public readonly TimeSpan SHOW_EVENTS = TimeSpan.FromDays(30);

        public AlarmsService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Returns alarms </summary>
        /// <param name="userDto">user who requests the alarms</param>
        /// <returns>List of alarms</returns>
        public List<AlarmDto> GetData(UserDto userDto) {
            var models = UnitOfWork.GetRepository<Alarm>().SearchFor(e => e.CreatedById == userDto.Id);
            return Mapper.Map<List<AlarmDto>>(models);
        }

        /// <summary>
        ///     Returns alarms for menu's tooltip </summary>
        /// <param name="date">date today</param>
        /// <param name="userDto">user who requests the alarms</param>
        /// <returns>List of alarms</returns>
        public List<AlarmDto> GetNearAlarms(DateTime date, UserDto userDto) {
            var resultDtos = new List<AlarmDto>();
            var dtos = this.GetData(userDto);
            // filters
            foreach (var alarm in dtos) {
                if (alarm.RepeatValue == null) {
                    if (alarm.Date >= date) {
                        if (alarm.Date - date < SHOW_EVENTS) {
                            resultDtos.Add(alarm);
                        }
                    } else {
                        // checks dates in client and server
                        if (Math.Abs((date - DateTime.Now).Days) < 1) {
                            // deletes old events
                            UnitOfWork.GetRepository<Alarm>().DeleteById(alarm.EntityId);
                            UnitOfWork.SaveChanges();
                        }
                    }
                } else {
                    if (alarm.RepeatType != null && alarm.RepeatValue > 0) {
                        while (alarm.Date - date < SHOW_EVENTS) {
                            if (alarm.Date >= date) {
                                resultDtos.Add((AlarmDto)alarm.Clone());
                            }
                            switch ((AlarmRepeatEnum)alarm.RepeatType) {
                                case AlarmRepeatEnum.Days:
                                    alarm.Date = alarm.Date.AddDays((byte)alarm.RepeatValue);
                                    break;
                                case AlarmRepeatEnum.Weeks:
                                    alarm.Date = alarm.Date.AddDays((byte)alarm.RepeatValue * 7);
                                    break;
                                case AlarmRepeatEnum.Months:
                                    alarm.Date = alarm.Date.AddMonths((byte)alarm.RepeatValue);
                                    break;
                                case AlarmRepeatEnum.Years:
                                    alarm.Date = alarm.Date.AddYears((byte)alarm.RepeatValue);
                                    break;
                                default:
                                    throw new Exception("undefined enum: " + (AlarmRepeatEnum)alarm.RepeatType);
                            }
                        }
                    }
                }
            }
            return resultDtos.OrderBy(e => e.Date).ToList();
        }

        /// <summary>
        ///     Creates or Updates alarm </summary>
        /// <param name="alarmDto">alarm DTO</param>
        /// <param name="userDto">user who updates the alarm</param>
        public void Save(AlarmDto alarmDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Alarm>();
            var model = rep.GetById(alarmDto.EntityId);
            if (model == null) {
                model = this.Mapper.Map<Alarm>(alarmDto);
                this.UnitOfWork.GetRepository<Alarm>().Insert(model, userDto.Id);
            } else {
                this.Mapper.Map(alarmDto, model);
                this.UnitOfWork.GetRepository<Alarm>().Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Deletes alarm by id </summary>
        /// <param name="alarmId">alarm id</param>
        /// <param name="userDto">user who deletes the alarm</param>
        public void Delete(Guid alarmId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Alarm>();
            var model = rep.GetById(alarmId);
            if (model != null && model.CreatedById == userDto.Id) {
                this.UnitOfWork.GetRepository<Alarm>().MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
