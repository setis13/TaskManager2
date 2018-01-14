using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class SubTask : BaseEntity {
        public Guid CompanyId { get; set; }
        public Guid TaskId { get; set; }

        public int Order { get; set; }
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Title { get; set; }
        [DataType("VARCHAR"), MaxLength(1024)]
        public string Description { get; set; }
        public Int64 ActualWorkTicks { get; set; }
        [NotMapped]
        public TimeSpan ActualWork {
            get { return TimeSpan.FromTicks(ActualWorkTicks); }
            set { ActualWorkTicks = value.Ticks; }
        }
        public Int64 TotalWorkTicks { get; set; }
        [NotMapped]
        public TimeSpan TotalWork {
            get { return TimeSpan.FromTicks(TotalWorkTicks); }
            set { TotalWorkTicks = value.Ticks; }
        }
        public float Progress { get; set; }
        public byte Status { get; set; }

        public ICollection<Comment> Comments { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }
    }
}
