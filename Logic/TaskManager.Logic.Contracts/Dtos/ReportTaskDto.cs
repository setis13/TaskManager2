using System;
using System.Collections.Generic;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportTaskDto : Task1Dto {
        public List<ReportCommentDto> ReportComments { get; set; } = new List<ReportCommentDto>();
        public List<ReportSubTaskDto> ReportSubTasks { get; set; } = new List<ReportSubTaskDto>();

        public static ReportTaskDto CreateById(Guid id) {
            return new ReportTaskDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
