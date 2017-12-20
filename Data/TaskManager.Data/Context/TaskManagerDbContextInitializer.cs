using System.Data.Entity;

namespace TaskManager.Data.Context {
    public class TaskManagerDbContextInitializer :
        MigrateDatabaseToLatestVersion<TaskManagerDbContext, TaskManagerDbContextConfiguration> {
    }
}
