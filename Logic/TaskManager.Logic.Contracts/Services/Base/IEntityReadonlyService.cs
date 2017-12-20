using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Services.Base {
    /// <summary>
    ///     Readonly service contract </summary>
    public interface IEntityReadonlyService<T> : IService where T : BaseDto {
        /// <summary>
        ///     Gets all DTOs list </summary>
        /// <returns>DTOs list</returns>
        List<T> GetAll();
        /// <summary>
        ///     Gets DTO by ID </summary>
        /// <param name="id">DTO ID</param>
        /// <returns>Dto</returns>
        T GetById(Guid id);
    }
}
