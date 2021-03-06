﻿using System;
using System.Collections.Generic;

namespace TaskManager.Logic.Dtos {
    public class Task1Dto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Priority { get; set; }
        public TimeSpan ActualWork { get; set; }
        public TimeSpan? TotalWork { get; set; }
        public float Progress { get; set; } 
        public byte Status { get; set; }
        public bool Favorite { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<SubTaskDto> SubTasks { get; set; } = new List<SubTaskDto>();
        public List<FileDto> Files { get; set; } = new List<FileDto>();
    }
}
