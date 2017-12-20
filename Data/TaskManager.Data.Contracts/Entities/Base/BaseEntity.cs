using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Common.Identity;

namespace TaskManager.Data.Contracts.Entities.Base {
    /// <summary>
    ///     Base entity implementation </summary>
    public class BaseEntity {
        /// <summary>
        ///     Entity PK </summary>
        [Key]
        public Guid EntityId { get; set; } = Guid.NewGuid();
        /// <summary>
        ///     First time creation date </summary>
        [Required, Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }/// <summary>
        ///     Fist time created by person id </summary>
        public Guid CreatedById { get; set; }
        public virtual TaskManagerUser CreatedBy { get; set; }
        /// <summary>
        ///     Last modified date </summary>
        [Required]
        public DateTime LastModifiedDate { get; set; }
        /// <summary>
        ///     Last modified by person id </summary>
        public Guid LastModifiedById { get; set; }
        public virtual TaskManagerUser LastModifiedBy { get; set; }
        /// <summary>
        ///     Marked as deleted </summary>
        [Required]
        public bool IsDeleted { get; set; }
    }
}