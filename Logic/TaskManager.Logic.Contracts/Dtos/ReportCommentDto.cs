using System;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ReportCommentDto : CommentDto {
        public float? DeltaProgress { get; set; }
    }
}
