using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;

namespace source_service.Service.Interface
{
    public interface IFileStorageService
    {
        public Task<FileStorage> GetFileStorageBySourceId(string sourceId);
        public Task<FileStorage> AddFileStorage(FileStorage fileStorage);
        public Task<FileStorage> DeleteFileStorage(string sourceId);


    }
}