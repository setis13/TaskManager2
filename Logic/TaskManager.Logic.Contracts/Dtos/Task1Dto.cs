using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class Task1Dto : BaseDto {
        public Guid CompanyId { get; set; }
        public Guid ProjectId { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Priority { get; set; }
        public TimeSpan ActualWork { get; set; }
        public TimeSpan TotalWork { get; set; }
        public float Progress { get; set; } 
        public byte Status { get; set; }
    }
}
