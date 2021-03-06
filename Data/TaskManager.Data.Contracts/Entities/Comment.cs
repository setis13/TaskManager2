﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Data.Entities {
    public class Comment : BaseEntity {
        public Guid CompanyId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? SubTaskId { get; set; }

        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public Int64? ActualWorkTicks { get; set; }
        [NotMapped]
        public TimeSpan? ActualWork {
            get { return ActualWorkTicks != null ? 
                    TimeSpan.FromTicks(ActualWorkTicks.Value) :
                    (TimeSpan?) null; }
            set { ActualWorkTicks = value?.Ticks; }
        }
        public float? Progress { get; set; }
        [DataType("VARCHAR(MAX)")]
        public string Description { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }
        [ForeignKey("SubTaskId")]
        public SubTask SubTask { get; set; }
    }
}
