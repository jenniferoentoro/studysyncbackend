using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using source_service.Dtos.Source;
using source_service.Model;

namespace source_service.Service.Interface
{
    public interface IFileService
    {
        Task<UploadFile> Upload(IFormFile fileModel, string idUser, string privacy);

        Task<Download> GetFile(string name);
    }
}