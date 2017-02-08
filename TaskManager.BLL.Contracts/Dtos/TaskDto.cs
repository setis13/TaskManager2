using System;
using TaskManager.BLL.Contracts.Dtos.Base;

namespace TaskManager.BLL.Contracts.Dtos {
    public class TaskDto : BaseDto {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public byte Important { get; set; }
        public byte Progress { get; set; }
        public DateTime Date { get; set; }
        public Guid SubprojectId { get; set; }
    }
}
