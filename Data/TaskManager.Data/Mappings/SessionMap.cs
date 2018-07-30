using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Session entity map </summary>
    public class SessionMap : EntityTypeConfiguration<Session> {
        /// <summary>
        ///     Session map instance </summary>
        public SessionMap() : base() {
            this.ToTable("Session");
        }
    }
}