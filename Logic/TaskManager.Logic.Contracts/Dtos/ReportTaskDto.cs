using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Logic.Dtos {
    public class ReportTaskDto : Task1Dto {
        public List<ReportCommentDto> ReportComments { get; set; } = new List<ReportCommentDto>();
        public List<ReportSubTaskDto> ReportSubTasks { get; set; } = new List<ReportSubTaskDto>();

        public TimeSpan SumActualWork => new TimeSpan(
            ReportComments
                .Where(e => e.ActualWork != null)
                .Select(e => e.ActualWork.Value.Ticks)
                .DefaultIfEmpty(0)
                .Sum() +
             ReportSubTasks
                .Select(e => e.SumActualWork.Ticks)
                .DefaultIfEmpty(0)
                .Sum());

        public static ReportTaskDto CreateById(Guid id) {
            return new ReportTaskDto() {
                EntityId = id,
                Title = id.ToString().Substring(0, 4)
            };
        }
    }
}
