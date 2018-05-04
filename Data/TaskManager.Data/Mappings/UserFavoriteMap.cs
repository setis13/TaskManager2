using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Contracts.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     UserFavorite entity map </summary>
    public class UserFavoriteMap : EntityTypeConfiguration<UserFavorite> {
        /// <summary>
        ///     UserFavorite map instance </summary>
        public UserFavoriteMap() : base() {
            this.ToTable("UserFavorite");
        }
    }
}