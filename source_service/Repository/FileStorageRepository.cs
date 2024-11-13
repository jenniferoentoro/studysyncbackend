using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace source_service.Repository
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly IMongoCollection<FileStorage> _fileStorage;

        public FileStorageRepository(IOptions<DatabaseConfiguration> fileStorageConfiguration)
        {
            var mongoClient = new MongoClient(fileStorageConfiguration.Value.ConnectionString);
            var database = mongoClient.GetDatabase(fileStorageConfiguration.Value.DatabaseNameSource);

            _fileStorage = database.GetCollection<FileStorage>(fileStorageConfiguration.Value.FileStorageCollectionName);
        }

        public async Task<FileStorage> AddFileStorage(FileStorage fileStorage)
        {
            if (fileStorage == null)
            {
                throw new ArgumentNullException(nameof(fileStorage));
            }
            await _fileStorage.InsertOneAsync(fileStorage);
            return fileStorage;
        }

        public async Task<FileStorage> DeleteFileStorage(string sourceId)
        {
            var fileStorage = await _fileStorage.FindOneAndDeleteAsync(f => f.SourceId == sourceId);

            if (fileStorage == null)
            {
                throw new Exception("FileStorage not found");
            }

            return fileStorage;
        }


        public async Task<FileStorage> GetFileStorageBySourceId(string sourceId)
        {
            var fileStorage = await _fileStorage.Find(f => f.SourceId == sourceId).FirstOrDefaultAsync();
            if (fileStorage == null)
            {
                throw new Exception($"FileStorage not found for SourceId: {sourceId}");
            }

            return fileStorage;
        }
    }
}