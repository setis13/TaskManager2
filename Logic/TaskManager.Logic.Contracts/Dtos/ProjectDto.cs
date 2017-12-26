using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class ProjectDto : BaseDto {
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
    }
}
