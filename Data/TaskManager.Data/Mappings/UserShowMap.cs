using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     UserShow entity map </summary>
    public class UserShowMap : EntityTypeConfiguration<UserShow> {
        /// <summary>
        ///     UserShow map instance </summary>
        public UserShowMap() : base() {
            this.ToTable("UserShow");
        }
    }
}