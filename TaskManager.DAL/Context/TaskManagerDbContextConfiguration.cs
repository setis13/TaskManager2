using System.Data.Entity.Migrations;

namespace TaskManager.DAL.Context {
    /// <summary>
    ///     Database context configuration </summary>
    public sealed class TaskManagerDbContextConfiguration : DbMigrationsConfiguration<TaskManagerDbContext> {
        /// <summary>
        ///     Cretes configutation instance </summary>
        public TaskManagerDbContextConfiguration() {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        ///     On db context seed </summary>
        /// <param name="context"></param>
        protected override void Seed(TaskManagerDbContext context) {
            
            base.Seed(context);
        }
    }
}