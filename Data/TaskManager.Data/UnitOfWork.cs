using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Context;
using TaskManager.Data.Contracts.Entities.Base;
using TaskManager.Data.Contracts.Repositories.Base;
using TaskManager.Data.Identity;
using TaskManager.Data.Repositories.Base;

namespace TaskManager.Data {
    /// <summary>
    ///     UOW interface implementation </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable {
        /// <summary>
        ///     Holds registered entity repositories </summary>
        private readonly Dictionary<Type, IRepository> entityRepositories;

        /// <summary>
        ///     Gets application db context instance </summary>
        public ITaskManagerDbContext Context { get; }

        /// <summary>
        ///     Gets Role Store </summary>
        public IRoleStore<TaskManagerRole, Guid> RoleStore { get; }
        /// <summary>
        ///     Gets User Store </summary>
        public IUserStore<TaskManagerUser, Guid> UserStore { get; }

        /// <summary>
        ///     Creates UOW instance </summary>
        /// <param name="context">Application db context</param>
        public UnitOfWork(ITaskManagerDbContext context, TaskManagerRoleStore roleStore, TaskManagerUserStore userStore) {
            entityRepositories = new Dictionary<Type, IRepository>();
            Context = context;
            RoleStore = roleStore;
            UserStore = userStore;
        }

        /// <summary>
        ///     Gets repository by entity type </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        public IRepository<T> GetRepository<T>() where T : BaseEntity {
            // checks if repository exist in cache
            if (entityRepositories.ContainsKey(typeof(T)))
                return entityRepositories[typeof(T)] as IRepository<T>;
            // if not then create a new instance and add to cache
            var repositoryType = typeof(Repository<>).MakeGenericType(typeof(T));
            var repository = (IRepository<T>)Activator.CreateInstance(repositoryType, this.Context);
            entityRepositories.Add(typeof(T), repository);
            return repository;
        }

        /// <summary>
        ///     The roll back. </summary>
        public void RollBack() {
            var changedEntries =
                this.Context.DbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified)) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added)) {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted)) {
                entry.State = EntityState.Unchanged;
            }
        }

        /// <summary>
        ///     The commit. </summary>
        public void SaveChanges() {
            try {
                this.Context.DbContext.SaveChanges();
            } catch (DbEntityValidationException ex) {
                var sb = new StringBuilder();

                foreach (DbEntityValidationResult failure in ex.EntityValidationErrors) {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType().Name);
                    foreach (DbValidationError error in failure.ValidationErrors) {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb.ToString(), ex);
                // Add the original exception as the innerException
            }
        }

        public void Dispose() {
            Context.Dispose();
        }
    }
}