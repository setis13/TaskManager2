using System.Data.Entity.ModelConfiguration;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.DAL.Mappings {
    public class CommentMap : EntityTypeConfiguration<Comment> {
        public CommentMap() {
            base.ToTable("Comment");
        }
    }
}
