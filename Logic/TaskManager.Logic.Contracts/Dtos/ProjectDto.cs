using System;

namespace TaskManager.Logic.Dtos {
    public class ProjectDto : BaseDto {
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }
}
