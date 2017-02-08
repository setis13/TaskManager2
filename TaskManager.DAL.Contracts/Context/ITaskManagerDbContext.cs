using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.DAL.Contracts.Context {
    /// <summary>
    ///     TaskManager Database context interface </summary>
    public interface ITaskManagerDbContext : IDisposable {
        /// <summary>
        ///     Gets generic db context instance </summary>
        DbContext DbContext { get; }
        
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}