using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Contracts.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     TaskUsers entity map </summary>
    public class TaskUserMap : EntityTypeConfiguration<TaskUser> {
        /// <summary>
        ///     TaskUsers map instance </summary>
        public TaskUserMap() : base() {
            this.ToTable("TaskUser");
        }
    }
}