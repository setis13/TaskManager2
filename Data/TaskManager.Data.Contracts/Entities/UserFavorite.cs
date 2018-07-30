using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Identity;

namespace TaskManager.Data.Entities {
    public class UserFavorite : BaseEntity {
        [Index("IX_UserFavorite", 1, IsUnique = true)]
        public Guid UserId { get; set; }
        [Index("IX_UserFavorite", 2, IsUnique = true)]
        public Guid? TaskId { get; set; }
        [Index("IX_UserFavorite", 3, IsUnique = true)]
        public Guid? SubTaskId { get; set; }

        [ForeignKey("UserId")]
        public TaskManagerUser User { get; set; }
        [ForeignKey("TaskId")]
        public Task1 Task { get; set; }
        [ForeignKey("SubTaskId")]
        public SubTask SubTask { get; set; }
    }
}
