using System.Data.Entity.ModelConfiguration;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.DAL.Mappings {
    public class TaskMap : EntityTypeConfiguration<Task> {
        public TaskMap() {
            base.ToTable("Task");
        }
    }
}
