using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class ReportService : HostService<IReportService>, IReportService {
        public ReportService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        public List<ReportProjectDto> GetSingleDayData(DateTime date, UserDto user) {
            var projectDtos = new List<ReportProjectDto>();
            var taskDtos = new List<ReportTaskDto>();
            var subtaskDtos = new List<ReportSubTaskDto>();
            var commentTaskDtos = new List<ReportCommentDto>();
            var commentSubtaskDtos = new List<ReportCommentDto>();

            // gets all time of day
            var start = date.Date;
            var end = date.Date.AddDays(1).AddMilliseconds(-1);

            var commentRep = UnitOfWork.GetRepository<Comment>();
            var subtaskRep = UnitOfWork.GetRepository<SubTask>();
            var taskRep = UnitOfWork.GetRepository<Task1>();
            var projectRep = UnitOfWork.GetRepository<Project>();

            var comments = commentRep.SearchFor(e => 
                e.CompanyId == user.CompanyId &&
                e.CreatedById == user.Id &&
                start <= e.Date && e.Date <= end 
            ).ToList();

            foreach(var group in comments.Where(e => e.TaskId != null).GroupBy(e => e.TaskId)) {
                var oldComment = commentRep.SearchFor(e =>
                    e.CompanyId == user.CompanyId &&
                    start > e.Date
                ).FirstOrDefault();
                foreach(Comment comment in group) {
                    
                }
            }
        }
    }
}
