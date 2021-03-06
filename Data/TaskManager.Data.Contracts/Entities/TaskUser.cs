﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Identity;

namespace TaskManager.Data.Entities {
    /// <summary>
    ///     Responsible </summary>
    public class TaskUser : BaseEntity {

        [Index("IX_TaskUser", 1, IsUnique = true)]
        public Guid TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }

        [Index("IX_TaskUser", 2, IsUnique = true)]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public TaskManagerUser User { get; set; }
    }
}
