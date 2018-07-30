using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Alarm entity map </summary>
    public class AlarmMap : EntityTypeConfiguration<Alarm> {
        /// <summary>
        ///     Alarm map instance </summary>
        public AlarmMap() : base() {
            this.ToTable("Alarm");
        }
    }
}