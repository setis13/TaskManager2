using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class TodoDto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Priority { get; set; }
    }
}
