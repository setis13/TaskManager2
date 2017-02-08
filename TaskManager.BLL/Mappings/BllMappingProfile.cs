using AutoMapper;
using TaskManager.BLL.Contracts.Dtos;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.BLL.Mappings {
    /// <summary>
    ///     Mapping configuration </summary>
    public class BllMappingProfile : Profile {

        protected override void Configure() {
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Subproject, SubprojectDto>().ReverseMap();
            CreateMap<Task, TaskDto>().ReverseMap();
        }
    }
}