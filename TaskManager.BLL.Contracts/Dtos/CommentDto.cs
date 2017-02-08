using System;
using TaskManager.BLL.Contracts.Dtos.Base;

namespace TaskManager.BLL.Contracts.Dtos {
    public class CommentDto : BaseDto {
        public string Text { get; set; }
        public TimeSpan? Hours { get; set; }
        public Guid TaskId { get; set; }
    }
}
