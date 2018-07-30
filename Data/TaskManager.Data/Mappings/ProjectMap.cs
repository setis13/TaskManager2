using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Project entity map </summary>
    public class ProjectMap : EntityTypeConfiguration<Project> {
        /// <summary>
        ///     Project map instance </summary>
        public ProjectMap() : base() {
            this.ToTable("Project");
        }
    }
}