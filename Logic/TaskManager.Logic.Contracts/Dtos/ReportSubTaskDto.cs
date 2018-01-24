using System;
using System.Collections.Generic;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportSubTaskDto : SubTaskDto {
        public List<ReportCommentDto> ReportComments { get; set; } = new List<ReportCommentDto>();

        public static ReportSubTaskDto CreateById(Guid id) {
            return new ReportSubTaskDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
