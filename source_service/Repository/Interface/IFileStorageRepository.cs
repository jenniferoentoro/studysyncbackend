using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;

namespace source_service.Repository.Interface
{
    public interface IFileStorageRepository
    {
        Task<FileStorage> GetFileStorageBySourceId(string sourceId);
        Task<FileStorage> AddFileStorage(FileStorage fileStorage);

        Task<FileStorage> DeleteFileStorage(string sourceId);

    }
}