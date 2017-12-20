using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Services.Base {
    /// <summary>
    ///     Generic CRUD service contract </summary>
    /// <typeparam name="TDto">DTO type</typeparam>
    public interface ICrudService<TDto> : IReadonlyService<TDto> where TDto : BaseDto {
        /// <summary>
        ///     Save entity to database </summary>
        /// <param name="dto">The DTO</param>
        void Save(TDto dto);
        /// <summary>
        ///     Delete entity from database </summary>
        /// <param name="dto">The DTO</param>
        void Delete(TDto dto);
        /// <summary>
        ///     Delete entiy from database by Id </summary>
        /// <param name="id">The DTO Id</param>
        void DeleteById(Guid id);
        /// <summary>
        ///     Delete entities from database by ids </summary>
        /// <param name="ids">Entity ids</param>
        void DeleteByIds(List<Guid> ids);
    }
}