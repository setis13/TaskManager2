using TaskManager.DAL.Contracts.Context;
using TaskManager.DAL.Contracts.Entities.Base;
using TaskManager.DAL.Contracts.Repositories;

namespace TaskManager.DAL.Contracts {
    /// <summary>
    ///     UOW contract
    /// </summary>
    public interface IUnitOfWork {
        /// <summary>
        ///     Gets application context instance </summary>
        ITaskManagerDbContext Context { get; }

        /// <summary>
        ///     Get repository by entity type </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IEntityRepository<T> GetEntityRepository<T>() where T : BaseEntity;

        /// <summary>
        ///     Rollback uncommited changes </summary>
        void RollBack();

        /// <summary>
        ///     Commit changes </summary>
        void SaveChanges();
    }
}