using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Contracts.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Task entity map </summary>
    public class TaskMap : EntityTypeConfiguration<Task1> {
        /// <summary>
        ///     Task map instance </summary>
        public TaskMap() : base() {
            this.ToTable("Task");
        }
    }
}