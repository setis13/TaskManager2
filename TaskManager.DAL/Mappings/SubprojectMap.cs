using System.Data.Entity.ModelConfiguration;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.DAL.Mappings {
    public class SubprojectMap : EntityTypeConfiguration<Subproject> {
        public SubprojectMap() {
            base.ToTable("Subproject");
        }
    }
}
