using Microsoft.AspNet.Identity;
using System;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Identity;
using TaskManager.Data.Repositories;

namespace TaskManager.Data {
    /// <summary>
    ///     UOW contract </summary>
    public interface IUnitOfWork {
        /// <summary>
        ///     Gets application context instance </summary>
        ITaskManagerDbContext Context { get; }
        /// <summary>
        ///     Gets Role Store </summary>
        IRoleStore<TaskManagerRole, Guid> RoleStore { get; }
        /// <summary>
        ///     Gets User Store </summary>
        IUserStore<TaskManagerUser, Guid> UserStore { get; }

        /// <summary>
        ///     Gets repository by entity type </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        /// <summary>
        ///     Rollbacks uncommited changes </summary>
        void RollBack();
        /// <summary>
        ///     Commits changes </summary>
        void SaveChanges();
    }
}
