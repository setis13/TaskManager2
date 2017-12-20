using System;
using AutoMapper;

namespace TaskManager.Logic.Mappings {
    /// <summary>
    ///     Mapping configuration </summary>
    public class BllMappingProfile : Profile {

        public BllMappingProfile() {
            //CreateMap<LogRulesDto, LogRulesViewModel>().ConvertUsing<LogBook.Logic.Converters.Rules.DtoToViewModel>();
        }
    }
}