using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using source_service.Dtos.Source;
using source_service.Model;
using source_service.Service.Interface;

namespace source_service.Service
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;


        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<UploadFile> Upload(IFormFile fileModel, string idUser, string privacy)
        {
            var containerName = privacy == "public" ? "studysyncsources" : "studysyncsourceprivate";
            var containerInstance = _blobServiceClient.GetBlobContainerClient(containerName);
            string fileName = $"{idUser}*{fileModel.FileName}*{DateTime.Now.ToString("dd-MM-yyyy")}*{Guid.NewGuid().ToString()}{Path.GetExtension(fileModel.FileName)}";



            var blobInstance = containerInstance.GetBlobClient(fileName);

            await blobInstance.UploadAsync(fileModel.OpenReadStream());

            if (privacy == "public")
            {
                return new UploadFile
                {
                    FileName = fileName,
                    UrlFile = blobInstance.Uri.ToString(),
                };
            }
            else
            {
                var sasUri = GenerateSasUri(blobInstance);

                return new UploadFile
                {
                    FileName = fileName,
                    UrlFile = blobInstance.Uri.ToString(),
                    Token = sasUri.ToString()
                };
            }

        }

        private Uri GenerateSasUri(BlobClient blobClient)
        {
            if (!blobClient.CanGenerateSasUri)
            {
                throw new InvalidOperationException("BlobClient is not authorized with Shared Key credentials.");
            }
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5)
            };
            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            return blobClient.GenerateSasUri(sasBuilder);
        }

        public async Task<Download?> GetFile(string name)
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("studysyncsources");

            var blobInstance = containerInstance.GetBlobClient(name);

            // var download = await blobInstance.DownloadAsync();
            // return download.Value.Content;

            if (await blobInstance.ExistsAsync())
            {
                var data = await blobInstance.OpenReadAsync();
                Stream blobContent = data;

                var content = await blobInstance.DownloadContentAsync();

                string contentType = content.Value.Details.ContentType;


                return new Download
                {
                    File = blobContent,
                    ContentType = contentType
                };
            }
            return null;

        }

        public async Task<DownloadResult?> GetFileWithDetails(string name)
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("studysyncsources");
            var blobInstance = containerInstance.GetBlobClient(name);

            if (await blobInstance.ExistsAsync())
            {
                var data = await blobInstance.OpenReadAsync();
                Stream blobContent = data;

                // Determine content type based on file extension
                var content = await blobInstance.DownloadContentAsync();

                string contentType = content.Value.Details.ContentType;



                // Populate other properties of the DTO as needed
                var fileDetails = new DownloadResult
                {
                    FileContent = blobContent,
                    FileName = name
                };

                return fileDetails;
            }

            return null;
        }

    }


}