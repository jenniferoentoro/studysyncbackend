using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace source_service.Model
{
    public class Source
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public String Title { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public String UrlFile { get; set; } = string.Empty;

        public String FileName { get; set; } = string.Empty;

        public string CategoryId { get; set; }
        public string UserId { get; set; }

        public string privacy { get; set; }

    }
}