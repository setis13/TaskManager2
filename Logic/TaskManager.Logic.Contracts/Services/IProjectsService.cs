using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Projects Service interface. </summary>
    public interface IProjectsService : IService {
        /// <summary>
        ///     Returns projects </summary>
        /// <param name="userDto">user who requests the projects</param>
        /// <returns>List of projects</returns>
        List<ProjectDto> GetData(UserDto userDto);
        /// <summary>
        ///     Creates or Updates project </summary>
        /// <param name="projectDto">project DTO</param>
        /// <param name="userDto">user who updates the project</param>
        void Save(ProjectDto projectDto, UserDto userDto);
        /// <summary>
        ///     Deletes project by id </summary>
        /// <param name="id">project id</param>
        /// <param name="userDto">user who deletes the project</param>
        void Delete(Guid projectId, UserDto userDto);
    }
}