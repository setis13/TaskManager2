using System;

namespace TaskManager.Logic.Contracts.Dtos.Base {

    public class BaseDto {
        /// <summary>
        ///     .ctor </summary>
        public BaseDto() {
            EntityId = Guid.NewGuid();
        }

        public Guid EntityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid LastModifiedById { get; set; }
        public bool IsDeleted { get; set; }
    }
}
