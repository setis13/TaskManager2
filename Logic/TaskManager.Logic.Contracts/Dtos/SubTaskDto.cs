﻿using System;
using System.Collections.Generic;

namespace TaskManager.Logic.Dtos {
    public class SubTaskDto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid TaskId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan ActualWork { get; set; }
        public TimeSpan? TotalWork { get; set; }
        public float Progress { get; set; }
        public byte Status { get; set; }
        public bool Favorite { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<FileDto> Files { get; set; } = new List<FileDto>();
    }
}
