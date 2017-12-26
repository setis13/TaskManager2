using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class CommentDto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid TaskId { get; set; }
        public Guid SubTaskId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public TimeSpan ActualWork { get; set; }
    }
}
