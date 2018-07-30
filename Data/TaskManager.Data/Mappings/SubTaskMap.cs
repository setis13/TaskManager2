using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     SubTask entity map </summary>
    public class SubTaskMap : EntityTypeConfiguration<SubTask> {
        /// <summary>
        ///     SubTask map instance </summary>
        public SubTaskMap() : base() {
            this.ToTable("SubTask");
        }
    }
}