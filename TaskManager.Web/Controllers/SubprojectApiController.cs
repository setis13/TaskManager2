using System;
using System.Web.Http;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Dtos;
using TaskManager.BLL.Contracts.Services;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers {
    [RoutePrefix("api/Subproject")]
    public class SubprojectApiController : ApiController {

        private readonly ISubprojectService _service;

        public SubprojectApiController(IServicesHost servicesHost) {
            _service = servicesHost.GetService<ISubprojectService>();
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

        [Route("GetById/{subprojectId}"), HttpGet]
        public WebApiResunt GetById(Guid subprojectId) {
            try {
                var data = _service.GetById(subprojectId);
                return WebApiResunt.Succeed(data);
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }

        [Route("Delete/{subprojectId}"), HttpGet]
        public WebApiResunt Delete(Guid subprojectId) {
            try {
                _service.MarkAsDeletedId(subprojectId);
                return WebApiResunt.Succeed();
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }

        [Route("Save"), HttpPost]
        public WebApiResunt Save(SubprojectDto dto) {
            try {
                _service.Save(dto);
                return WebApiResunt.Succeed(dto);
            } catch (Exception e) {
                return WebApiResunt.Failed(e);
            }
        }
    }
}