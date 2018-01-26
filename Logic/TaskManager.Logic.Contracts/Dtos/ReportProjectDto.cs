using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportProjectDto : ProjectDto {
        public List<ReportTaskDto> ReportTasks { get; set; } = new List<ReportTaskDto>();

        public TimeSpan SumActualWork => new TimeSpan(ReportTasks
            .Select(e => e.SumActualWork.Ticks)
            .DefaultIfEmpty(0)
            .Sum());

        public static ReportProjectDto CreateById(Guid id) {
            return new ReportProjectDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
