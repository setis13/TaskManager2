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
        [DataType("VARCHAR"), MaxLength(1024), Required]
        public string Description { get; set; }
        public TimeSpan ActualWork { get; set; }
        public TimeSpan TotalWork { get; set; }
        public float Progress { get; set; }
        public byte Status { get; set; }

        public ICollection<Comment> Comments { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }
    }
}
