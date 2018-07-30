using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Data.Entities {
    public class Project : BaseEntity {
        public Guid CompanyId { get; set; }

        [DataType("VARCHAR"), MaxLength(32), Required]
        public string Title { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int Count { get; set; }
    }
}
