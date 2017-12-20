using System;
using TaskManager.Data.Contracts.Context;
using TaskManager.Data.Contracts.Entities.Base;
using TaskManager.Data.Contracts.Repositories.Base;

namespace TaskManager.Data.Contracts {
    /// <summary>
    ///     UOW contract </summary>
    public interface IUnitOfWork {
        /// <summary>
        ///     Gets application context instance </summary>
        ITaskManagerDbContext Context { get; }
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
