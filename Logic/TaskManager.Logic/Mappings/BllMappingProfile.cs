using AutoMapper;
using TaskManager.Data.Entities;
using TaskManager.Data.Identity;
using TaskManager.Logic.Dtos;

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
            CreateMap<FileDto, File1>().ReverseMap();
            CreateMap<AlarmDto, Alarm>().ReverseMap();

            CreateMap<Project, ReportProjectDto>();
            CreateMap<Task1, ReportTaskDto>();
            CreateMap<SubTask, ReportSubTaskDto>();
            CreateMap<Comment, ReportCommentDto>();
        }
    }
}