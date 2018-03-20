using System.Data.Entity.ModelConfiguration;
using TaskManager.Data.Contracts.Entities;

namespace TaskManager.Data.Mappings {
    /// <summary>
    ///     File entity map </summary>
    public class FileMap : EntityTypeConfiguration<File1> {
        /// <summary>
        ///     File map instance </summary>
        public FileMap() : base() {
            this.ToTable("File");
        }
    }
}