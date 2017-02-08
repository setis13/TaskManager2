using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Contracts.Entities.Base {
    public class BaseEntity {
        [Key]
        public Guid EntityId { get; set; }

        [Required, Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        [Required, Column(TypeName = "datetime2")]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public BaseEntity() {
            this.EntityId = Guid.NewGuid();
        }
    }
}
