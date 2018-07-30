using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Company entity map </summary>
    public class CompanyMap : EntityTypeConfiguration<Company> {
        /// <summary>
        ///     Company map instance </summary>
        public CompanyMap() : base() {
            this.ToTable("Company");
            this.HasMany(e => e.Users)
                .WithOptional(e => (Company)e.Company)
                .HasForeignKey(e => e.CompanyId);
        }
    }
}