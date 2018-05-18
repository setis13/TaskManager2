using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class AlarmDto : BaseDto, ICloneable {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public byte? RepeatType { get; set; }
        public byte? RepeatValue { get; set; }
        public bool Birthday { get; set; }
        public bool Holiday { get; set; }

        public object Clone() {
            return new AlarmDto() {
                EntityId = this.EntityId,
                CreatedDate = this.CreatedDate,
                CreatedById = this.CreatedById,
                LastModifiedDate = this.LastModifiedDate,
                LastModifiedById = this.LastModifiedById,
                IsDeleted = this.IsDeleted,

                Title = this.Title,
                Description = this.Description,
                Date = this.Date,
                RepeatType = this.RepeatType,
                RepeatValue = this.RepeatValue,
                Birthday = this.Birthday,
                Holiday = this.Holiday
            };
        }
    }
}
