using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class CommentDto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? SubTaskId { get; set; }
        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public TimeSpan? ActualWork { get; set; }
        public float? Progress { get; set; }
        public string Description { get; set; }
    }
}
