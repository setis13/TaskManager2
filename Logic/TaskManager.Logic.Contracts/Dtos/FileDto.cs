using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {
    public class FileDto : BaseDto {
        public Guid ParentId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
