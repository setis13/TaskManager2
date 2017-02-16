using System;
using TaskManager.BLL.Contracts.Dtos.Base;

namespace TaskManager.BLL.Contracts.Dtos {
    public class SubprojectDto : BaseDto {
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public double Hours { get; set; }
    }
}
