using System;
using System.Web.Http;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Dtos;
using TaskManager.BLL.Contracts.Services;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers {
    [RoutePrefix("api/Project")]
    public class ProjectApiController : ApiController {

        private readonly IProjectService _service;

        public ProjectApiController(IServicesHost servicesHost) {
            _service = servicesHost.GetService<IProjectService>();
        }

        [Route("GetAll"), HttpGet]
        public WebApiResunt GetAll() {
            try {
                var data = _service.GetAll();
                return WebApiResunt.Succeed(data);
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }

        [Route("GetById/{projectId}"), HttpGet]
        public WebApiResunt GetById(Guid projectId) {
            try {
                var data = _service.GetById(projectId);
                return WebApiResunt.Succeed(data);
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }

        [Route("Delete/{projectId}"), HttpGet]
        public WebApiResunt Delete(Guid projectId) {
            try {
                _service.MarkAsDeletedId(projectId);
                return WebApiResunt.Succeed();
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }

        [Route("Save"), HttpPost]
        public WebApiResunt Save(ProjectDto dto) {
            try {
                _service.Save(dto);
                return WebApiResunt.Succeed(dto);
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }
    }
}