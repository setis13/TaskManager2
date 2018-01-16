using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Comment : BaseEntity {
        public Guid CompanyId { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? SubTaskId { get; set; }

        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public Int64? ActualWorkTicks { get; set; }
        [NotMapped]
        public TimeSpan? ActualWork {
            get { return TimeSpan.FromTicks(ActualWorkTicks); }
            set { ActualWorkTicks = value.Ticks; }
        }
        public float? Progress { get; set; }
        [DataType("VARCHAR"), MaxLength(2048)]
        public string Description { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }
        [ForeignKey("SubTaskId")]
        public SubTask SubTask { get; set; }
    }
}
