using System;

namespace TaskManager.Data.Entities {
    public class File1 : BaseEntity {
        public Guid ParentId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
    }
}