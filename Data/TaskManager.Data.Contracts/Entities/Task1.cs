using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Task1 : BaseEntity {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }

        public int Index { get; set; }
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Title { get; set; }
        [DataType("VARCHAR(MAX)")]
        public string Description { get; set; }
        public byte Priority { get; set; }
        // more then 24 hours
        //http://qaru.site/questions/41643/how-do-i-map-timespan-with-greater-than-24-hours-to-sql-server-code-first
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
        public ICollection<SubTask> SubTasks { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
