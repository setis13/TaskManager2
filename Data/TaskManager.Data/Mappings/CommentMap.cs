using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Comment entity map </summary>
    public class CommentMap : EntityTypeConfiguration<Comment> {
        /// <summary>
        ///     Comment map instance </summary>
        public CommentMap() : base() {
            this.ToTable("Comment");
        }
    }
}