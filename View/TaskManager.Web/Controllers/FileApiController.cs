using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Controllers.Base;
using System.Net.Http;
using TaskManager.Web.Models;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Contracts.Dtos;
using System.Net.Http.Headers;
using System.Net;
using TaskManager.Logic.Contracts.Exceptions;
using TaskManager.Data.Contracts.Helpers;
using System.Linq;
using System.IO;
using TaskManager.Web.Helpers;

namespace TaskManager.Web.Controllers {
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    [RoutePrefix("api/File")]
    public class FileApiController : BaseApiController {

        private IFileService _service => this.ServicesHost.GetService<IFileService>();

        public FileApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/File/Attach </summary>
        [HttpPost, Route("Attach")]
        [Authorize]
        public async Task<WebApiResult> Attach() {
#if DEBUG
            await Task.Delay(300);
#endif
            try {
                if (!Request.Content.IsMimeMultipartContent()) {
                    return WebApiResult.Failed("Bad Request");
                }
                MultipartMemoryStreamProvider provider = await Request.Content.ReadAsMultipartAsync();
                var entityId = new Guid(HttpHeadersHelper.GetHeader(Request.Headers, "EntityId"));
                var existsFiles = _service.GetFileNames(entityId);
                var existsModels = _service.GetModels(entityId);
                // adds new files
                foreach (HttpContent file in provider.Contents) {
                    var fileName = file.Headers.ContentDisposition.Name.Trim('\"');
                    byte[] data = await file.ReadAsByteArrayAsync();
                    // a existing file
                    if (data.Length != 0) {
                        var fileDto = await _service.SaveFile(entityId, fileName, data);
                        _service.SaveModel(fileDto, base.GetUserDto());
                    }
                    // removes file
                    existsFiles.Remove(fileName);
                    // removes model
                    if (existsModels.Any(e => e.FileName == fileName)) {
                        existsModels.Remove(existsModels.First(e => e.FileName == fileName));
                    }
                }
                // removes unspecified files
                foreach (string fileName in existsFiles) {
                    _service.RemoveFile(entityId, fileName);
                }
                // removes unspecified models
                foreach (FileDto model in existsModels) {
                    _service.RemoveModel(model);
                }
                return WebApiResult.Succeed();
            } catch (IOException e) {
                var entityId = new Guid(HttpHeadersHelper.GetHeader(Request.Headers, "EntityId"));
                _service.RemoveModels(entityId);
                _service.RemoveFiles(entityId);
                return WebApiResult.Succeed();
            } catch (Exception e) {
                Logger.e("SaveFile", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/File/Download </summary>
        [HttpGet, Route("Download")]
        [Authorize]
        public HttpResponseMessage Download(Guid id) {
            try {
                FileDto fileDto = _service.GetFileById(id, base.GetUserDto());
                if (fileDto != null) {
                    var result = new HttpResponseMessage(HttpStatusCode.OK) {
                        Content = new ByteArrayContent(fileDto.Data)
                    };
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
                        FileName = fileDto.FileName
                    };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeHelper.GetMimeType(Path.GetExtension(fileDto.FileName)));
                    return result;
                } else {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            } catch (PermissionException e) {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            } catch (Exception e) {
                Logger.e("Download", e);
                return new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(e.Message)
                };
            }
        }
    }
}