using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskManager.Data.Identity;
using TaskManager.Data.Mappings;

namespace TaskManager.Data.Context {
    public class TaskManagerDbContext : 
        IdentityDbContext<TaskManagerUser, TaskManagerRole, Guid, TaskManagerUserLogin, TaskManagerUserRole,TaskManagerUserClaim>,
        ITaskManagerDbContext {

        public DbContext DbContext => this;

        /// <summary>
        ///     Creates context instance </summary>
        public TaskManagerDbContext() : base("DefaultConnection") {
            this.Configuration.LazyLoadingEnabled = true;
        }

        /// <summary>
        ///     Builds models </summary>
        /// <param name="modelBuilder">Models builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Identity
            modelBuilder.Entity<TaskManagerUser>().ToTable("Users");
            modelBuilder.Entity<TaskManagerRole>().ToTable("Roles");
            modelBuilder.Entity<TaskManagerUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<TaskManagerUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<TaskManagerUserClaim>().ToTable("UserClaims");

            modelBuilder.Configurations.Add(new SessionMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new TaskMap());
            modelBuilder.Configurations.Add(new SubTaskMap());
            modelBuilder.Configurations.Add(new TodoMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new TaskUserMap());
            modelBuilder.Configurations.Add(new UserFavoriteMap());
            modelBuilder.Configurations.Add(new UserShowMap());
            modelBuilder.Configurations.Add(new FileMap());
            modelBuilder.Configurations.Add(new AlarmMap());

            // Conventions
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
