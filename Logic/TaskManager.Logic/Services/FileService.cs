using AutoMapper;
using System.IO;
using System.Threading.Tasks;
using TaskManager.Data;
using System;
using System.Collections.Generic;
using TaskManager.Logic.Dtos;
using TaskManager.Data.Entities;
using System.Linq;
using System.Web;
using TaskManager.Common;

namespace TaskManager.Logic.Services {
    public class FileService : HostService, IFileService {

        private readonly string root = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\Files\\");

        public FileService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        public List<FileDto> GetModels(List<Guid> entityIds) {
            var models = UnitOfWork.GetRepository<File1>().SearchFor(e => entityIds.Contains(e.ParentId));
            return Mapper.Map<List<FileDto>>(models);
        }

        public List<FileDto> GetModels(Guid entityId) {
            var models = UnitOfWork.GetRepository<File1>().SearchFor(e => e.ParentId == entityId);
            return Mapper.Map<List<FileDto>>(models);
        }

        public byte[] GetFile(FileDto fileDto) {
            var fullPath = Path.Combine(root, fileDto.ParentId.ToString(), fileDto.FileName);
            return File.ReadAllBytes(fullPath);
        }

        public List<string> GetFileNames(Guid entityId) {
            var fullPath = Path.Combine(root, entityId.ToString());
            if (Directory.Exists(fullPath)) {
                return Directory.GetFiles(fullPath).Select(e => Path.GetFileName(e)).ToList();
            } else {
                return new List<string>();
            }
        }

        public FileDto GetFileById(Guid fileId, UserDto userDto) {
            var model = UnitOfWork.GetRepository<File1>().GetById(fileId);
            // TODO check permission
            //throw new PermissionException();
            if (model != null) {
                var fileDto = Mapper.Map<FileDto>(model);
                var fullPath = Path.Combine(root, fileDto.ParentId.ToString(), fileDto.FileName);
                fileDto.Data = File.ReadAllBytes(fullPath);
                return fileDto;
            } else {
                return null;
            }
        }

        public async Task<FileDto> SaveFile(Guid entityId, string fileName, byte[] data) {
            var fullPath = Path.Combine(root, entityId.ToString(), fileName);
            var dir = Path.GetDirectoryName(fullPath);
            if (Directory.Exists(dir) == false) {
                Directory.CreateDirectory(dir);
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.Create)) {
                await fs.WriteAsync(data, 0, data.Length);
            }

            var dto = new FileDto() {
                ParentId = entityId,
                FileName = fileName,
                Size = data.Length,
            };
            return dto;
        }

        /// <summary>
        ///     Creates or Updates file </summary>
        /// <param name="fileDto">file DTO</param>
        /// <param name="userDto">user who updates the file</param>
        public void SaveModel(FileDto fileDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<File1>();
            var model = rep.SearchFor(e => e.ParentId == fileDto.ParentId && e.FileName == fileDto.FileName).FirstOrDefault();
            if (model == null) {
                model = this.Mapper.Map<File1>(fileDto);
                rep.Insert(model, userDto.Id);
            } else {
                var entityId = model.EntityId; // saves id
                this.Mapper.Map(fileDto, model);
                model.EntityId = entityId; // restores id
                rep.Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
        }

        public void RemoveFile(Guid entityId, string fileName) {
            var fullPath = Path.Combine(root, entityId.ToString(), fileName);
            if (File.Exists(fullPath) == true) {
                File.Delete(fullPath);
            }
        }

        public void RemoveModel(Guid entityId, string fileName) {
            var rep = UnitOfWork.GetRepository<File1>();
            var model = rep.SearchFor(e => e.ParentId == entityId && e.FileName == fileName).FirstOrDefault();
            if (model != null) {
                rep.DeleteById(model.EntityId);
                this.UnitOfWork.SaveChanges();
            }
        }

        public void RemoveModel(FileDto fileDto) {
            var rep = UnitOfWork.GetRepository<File1>();
            var model = rep.GetById(fileDto.EntityId);
            if (model != null) {
                rep.DeleteById(model.EntityId);
                this.UnitOfWork.SaveChanges();
            }
        }

        public void RemoveModels(Guid entityId) {
            var files = this.GetModels(entityId);
            foreach (var file in files) {
                this.RemoveModel(file);
            }
        }

        public void RemoveFiles(Guid entityId) {
            var files = this.GetFileNames(entityId);
            foreach (var file in files) {
                this.RemoveFile(entityId, file);
            }
        }
    }
}
