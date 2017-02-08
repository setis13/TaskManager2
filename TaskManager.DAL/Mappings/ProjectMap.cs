using System.Data.Entity.ModelConfiguration;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.DAL.Mappings {
    public class ProjectMap : EntityTypeConfiguration<Project> {
        public ProjectMap() {
            base.ToTable("Project");
        }
    }
}
