using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.DAL.Contracts.Entities {
    public class Task : BaseEntity {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public byte Important { get; set; }
        public byte Progress { get; set; }
        public DateTime Date { get; set; }
        public Guid SubprojectId { get; set; }

        [ForeignKey("SubprojectId")]
        public Subproject Subproject { get; set; }
    }
}
