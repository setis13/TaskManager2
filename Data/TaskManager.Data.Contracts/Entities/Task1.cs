using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Task1 : Todo {
        //public Guid CompanyId { get; set; }
        //public Guid ProjectId { get; set; }

        public int Index { get; set; }
        //[DataType("VARCHAR"), MaxLength(64), Required]
        //public string Title { get; set; }
        //[DataType("VARCHAR"), MaxLength(1024)]
        //public string Description { get; set; }
        //public byte Priority { get; set; }
        public TimeSpan ActualWork { get; set; }
        public TimeSpan TotalWork { get; set; }
        public float Progress { get; set; }
        public byte Status { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<SubTask> SubTasks { get; set; }

        //[ForeignKey("CompanyId")]
        //public Company Company { get; set; }
        //[ForeignKey("ProjectId")]
        //public Project Project { get; set; }
    }
}
