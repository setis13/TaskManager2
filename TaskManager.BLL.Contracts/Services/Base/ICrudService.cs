using System;
using System.Collections.Generic;
using TaskManager.BLL.Contracts.Dtos.Base;

namespace TaskManager.BLL.Contracts.Services.Base {
    /// <summary>
    ///     Generic CRUD service contract </summary>
    /// <typeparam name="T">Model type</typeparam>
    public interface ICrudService<T> : IEntityReadonlyService<T> where T : BaseDto {

        /// <summary>
        ///     Save model </summary>
        /// <param name="model"></param>
        void Save(T model);

        /// <summary>
        ///     Delete model </summary>
        /// <param name="model">Model</param>
        void MarkAsDeleted(T model);

        /// <summary>
        ///     Delete mnodel by Id </summary>
        /// <param name="modelId">Model id</param>
        void MarkAsDeletedId(Guid modelId);

        /// <summary>
        ///     Delete mnodel by Ids </summary>
        /// <param name="modelIds">Model ids</param>
        void MarkAsDeletedIds(List<Guid> modelIds);

    }
}