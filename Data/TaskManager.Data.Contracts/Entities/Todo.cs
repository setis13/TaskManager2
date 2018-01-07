using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Todo : BaseEntity {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }

        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Title { get; set; }
        [DataType("VARCHAR"), MaxLength(1024)]
        public string Description { get; set; }
        public byte Priority { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}
