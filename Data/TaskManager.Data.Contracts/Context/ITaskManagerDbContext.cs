using System;
using System.Data.Entity;

namespace TaskManager.Data.Contracts.Context {
    /// <summary>
    ///     Database context interface </summary>
    public interface ITaskManagerDbContext : IDisposable {
        /// <summary>
        ///     Gets db context instance </summary>
        DbContext DbContext { get; }
    }
}
