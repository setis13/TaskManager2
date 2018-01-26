using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportSubTaskDto : SubTaskDto {
        public List<ReportCommentDto> ReportComments { get; set; } = new List<ReportCommentDto>();

        public TimeSpan SumActualWork => new TimeSpan(
            ReportComments
                .Where(e=>e.ActualWork!=null)
                .Select(e=>e.ActualWork.Value.Ticks)
                .DefaultIfEmpty(0)
                .Sum());

        public static ReportSubTaskDto CreateById(Guid id) {
            return new ReportSubTaskDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
