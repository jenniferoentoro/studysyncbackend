using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using source_service.Helpers;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace source_service.Repository
{
    public class SourceRepository : ISourceRepository
    {
        private readonly IMongoCollection<Source> _sources;

        public SourceRepository(IOptions<DatabaseConfiguration> sourceConfiguration)
        {
            var mongoClient = new MongoClient(sourceConfiguration.Value.ConnectionString);
            var database = mongoClient.GetDatabase(sourceConfiguration.Value.DatabaseNameSource);
            _sources = database.GetCollection<Source>(sourceConfiguration.Value.SourcesCollectionName);
        }
        public async Task<Source> AddSource(Source source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            await _sources.InsertOneAsync(source);
            return source;
        }

        public async Task<Source> DeleteSource(string id)
        {
            var source = await GetSource(id);
            if (source == null)
            {
                throw new Exception("Source not found");
            }

            await _sources.DeleteOneAsync(s => s.Id == id);
            return source;
        }

        public async Task<Source> GetSource(string id)
        {
            var source = await _sources.Find(s => s.Id == id).FirstOrDefaultAsync();
            if (source == null)
            {
                throw new Exception("Source not found" + id);
            }

            return source;
        }

        public async Task<IEnumerable<Source>> GetSources(QueryObjectSource query)
        {
            var filter = Builders<Source>.Filter.Eq(s => s.privacy, "public");

            if (!string.IsNullOrWhiteSpace(query?.Title))
            {
                filter &= Builders<Source>.Filter.Regex(s => s.Title, new MongoDB.Bson.BsonRegularExpression(query.Title, "i"));
            }

            if (query?.UserId != null)
            {
                filter &= Builders<Source>.Filter.Eq(s => s.UserId, query.UserId);
            }

            var sort = Builders<Source>.Sort.Ascending(s => s.Title);
            if (query != null && query.IsDecsending)
            {
                sort = Builders<Source>.Sort.Descending(s => s.Title);
            }

            var skip = (query?.PageNumber - 1) * query?.PageSize ?? 0;

            var sources = await _sources.Find(filter).Sort(sort).Skip(skip).ToListAsync();
            return sources;
        }


        public async Task<Source> UpdateSource(Source source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var options = new FindOneAndReplaceOptions<Source> { ReturnDocument = ReturnDocument.After };
            var filter = Builders<Source>.Filter.Eq(s => s.Id, source.Id);
            var updatedSource = await _sources.FindOneAndReplaceAsync(filter, source, options);

            if (updatedSource == null)
            {
                throw new Exception("Source not found or not updated");
            }

            return updatedSource;
        }


        public async Task<int> CountSources()
        {
            return (int)await _sources.CountDocumentsAsync(_ => true);
        }

        public async Task<IEnumerable<Source>> DeleteSourcesByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var filter = Builders<Source>.Filter.Eq(s => s.UserId, userId);
            var sourcesToDelete = await _sources.Find(filter).ToListAsync();

            if (sourcesToDelete.Count == 0)
            {
                throw new Exception("No sources found for the specified user ID");
            }

            var result = await _sources.DeleteManyAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Failed to delete sources for the specified user ID");
            }

            return sourcesToDelete;
        }

        public async Task<IEnumerable<Source>> GetSourcesByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var filter = Builders<Source>.Filter.Eq(s => s.UserId, userId);
            var sources = await _sources.Find(filter).ToListAsync();
            return sources;
        }
    }
}