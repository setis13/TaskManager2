using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     Todo1 entity map </summary>
    public class TodoMap : EntityTypeConfiguration<Todo> {
        /// <summary>
        ///     Todo1 map instance </summary>
        public TodoMap() : base() {
            this.ToTable("Todo");
        }
    }
}