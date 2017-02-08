using System.Data.Entity;
using TaskManager.DAL.Context;

namespace TaskManager.Web {
    public class AppBootstrapper {
        public static void Init() {
            // DB context init
            Database.SetInitializer(new TaskManagerDbContextInitializer());
        }
    }
}