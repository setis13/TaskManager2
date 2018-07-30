using System;
using System.Collections.Generic;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    /// <summary>
    ///     Generic CRUD service contract </summary>
    /// <typeparam name="TDto">DTO type</typeparam>
    public interface ICrudService<TDto> : IEntityReadonlyService<TDto> where TDto : BaseDto {
        /// <summary>
        ///     Saves entity to database </summary>
        /// <param name="dto">The DTO</param>
        /// <param name="userId">User ID</param>
        void Save(TDto dto, Guid userId);
        /// <summary>
        ///     Deletes entity from database </summary>
        /// <param name="dto">The DTO</param>
        void Delete(TDto dto);
        /// <summary>
        ///     Deletes entiy from database by Id </summary>
        /// <param name="id">The DTO Id</param>
        void DeleteById(Guid id);
        /// <summary>
        ///     Deletes entities from database by ids </summary>
        /// <param name="ids">Entity ids</param>
        void DeleteByIds(List<Guid> ids);
    }
}