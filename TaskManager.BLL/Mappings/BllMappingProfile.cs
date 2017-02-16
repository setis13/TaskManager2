using System;
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
            CreateMap<Task, TaskDto>().ReverseMap();

            CreateMap<Subproject, SubprojectDto>().ForMember(s => s.Hours, opt => opt.ResolveUsing(s => s.Hours.TotalHours));
            CreateMap<SubprojectDto, Subproject>().ForMember(s => s.Hours, opt => opt.ResolveUsing(s => TimeSpan.FromHours(s.Hours)));
        }
    }
}