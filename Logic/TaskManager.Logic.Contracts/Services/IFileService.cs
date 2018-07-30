using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    public interface IFileService : IService {
        byte[] GetFile(FileDto fileDto);
        List<string> GetFileNames(Guid entityId);
        FileDto GetFileById(Guid fileId, UserDto userDto);
        List<FileDto> GetModels(List<Guid> entityIds);
        List<FileDto> GetModels(Guid entityId);
        Task<FileDto> SaveFile(Guid entityId, string fileName, byte[] data);
        void SaveModel(FileDto fileDto, UserDto userDto);
        void RemoveFile(Guid entityId, string fileName);
        void RemoveModel(Guid entityId, string fileName);
        void RemoveModel(FileDto fileDto);
        void RemoveModels(Guid entityId);
        void RemoveFiles(Guid entityId);
    }
}
