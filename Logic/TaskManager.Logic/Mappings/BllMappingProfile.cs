using System;
using AutoMapper;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts.Dtos;

namespace TaskManager.Logic.Mappings {
    /// <summary>
    ///     Mapping configuration </summary>
    public class BllMappingProfile : Profile {

        public BllMappingProfile() {
            CreateMap<UserDto, TaskManagerUser>().ReverseMap();
            CreateMap<CompanyDto, Company>().ReverseMap();
            CreateMap<ProjectDto, Project>().ReverseMap();
            CreateMap<Task1Dto, Task1>().ReverseMap();
            CreateMap<SubTaskDto, SubTask>().ReverseMap();
            CreateMap<TodoDto, Todo>().ReverseMap();
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}