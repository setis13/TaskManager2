using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Identity;

namespace TaskManager.Data.Entities {
    public class UserShow : BaseEntity {
        [Index("IX_UserShow", 1, IsUnique = true)]
        public Guid UserId { get; set; }
        [Index("IX_UserShow", 2, IsUnique = true)]
        public Guid? CommentId { get; set; }

        [ForeignKey("UserId")]
        public TaskManagerUser User { get; set; }
        [ForeignKey("CommentId")]
        public Comment comment { get; set; }
    }
}
