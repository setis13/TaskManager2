using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Contracts.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Company entity map </summary>
    public class CompanyMap : EntityTypeConfiguration<Company> {
        /// <summary>
        ///     Company map instance </summary>
        public CompanyMap() : base() {
            this.ToTable("Company");
        }
    }
}