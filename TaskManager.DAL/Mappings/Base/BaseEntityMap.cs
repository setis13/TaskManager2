using System.Data.Entity.ModelConfiguration;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.DAL.Mappings.Base {
    /// <summary>
    ///     Base entities mapping
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class BaseEntityMap<T> : EntityTypeConfiguration<T> where T : BaseEntity {
        /// <summary>
        ///     Create map instance
        /// </summary>
        public BaseEntityMap() {
        }
    }
}