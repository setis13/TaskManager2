using System;
using System.Collections.Generic;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportProjectDto : ProjectDto {
        public List<ReportTaskDto> ReportTasks { get; set; } = new List<ReportTaskDto>();

        public static ReportProjectDto CreateById(Guid id) {
            return new ReportProjectDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
