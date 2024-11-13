using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Service.Interface;

namespace source_service.Service
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IFileStorageRepository _fileStorageRepository;

        public FileStorageService(IFileStorageRepository fileStorageRepository)
        {
            _fileStorageRepository = fileStorageRepository;
        }

        public async Task<FileStorage> AddFileStorage(FileStorage fileStorage)
        {
            return await _fileStorageRepository.AddFileStorage(fileStorage);
        }

        public async Task<FileStorage> DeleteFileStorage(string sourceId)
        {
            return await _fileStorageRepository.DeleteFileStorage(sourceId);
        }

        public async Task<FileStorage> GetFileStorageBySourceId(string sourceId)
        {
            return await _fileStorageRepository.GetFileStorageBySourceId(sourceId);
        }
    }
}