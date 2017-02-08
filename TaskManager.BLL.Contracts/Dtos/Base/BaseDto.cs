using System;

namespace TaskManager.BLL.Contracts.Dtos.Base {
    /// <summary>
    ///     Base DTO </summary>
    /// <remarks> Represent database object </remarks>
    public class BaseDto {
        /// <summary>
        ///     .ctor </summary>
        public BaseDto() {
            EntityId = Guid.NewGuid();
        }

        /// <summary>
        ///     Entity ID </summary>
        public Guid EntityId { get; set; }
        /// <summary>
        ///     First time creation date </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        ///     Fist time created by person id </summary>
        public Guid CreatedById { get; set; }
        /// <summary>
        ///     Last modified date </summary>
        public DateTime LastModifiedDate { get; set; }
        /// <summary>
        ///     Last modified by person id </summary>
        public Guid LastModifiedById { get; set; }
        /// <summary>
        ///     Marked as deleted </summary>
        public bool IsDeleted { get; set; }
    }
}