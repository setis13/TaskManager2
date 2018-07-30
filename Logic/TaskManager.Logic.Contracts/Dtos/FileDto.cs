using System;

namespace TaskManager.Logic.Dtos {
    public class FileDto : BaseDto {
        public Guid ParentId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
