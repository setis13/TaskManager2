using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class ReportService : HostService<IReportService>, IReportService {
        public ReportService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

    }
}
