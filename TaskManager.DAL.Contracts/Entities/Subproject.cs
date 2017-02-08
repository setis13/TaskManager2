using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.DAL.Contracts.Entities {
    public class Subproject : BaseEntity {
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public TimeSpan Hours { get; set; }

        [ForeignKey("ProjectId")]
        public Entities.Project Project { get; set; }
    }
}
