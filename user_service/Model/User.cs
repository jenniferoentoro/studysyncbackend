using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using user_service.Model;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace user_service.model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public String Grade { get; set; }

        [Required]
        public string School { get; set; } = null!;

        [Required]
        public String Role { get; set; }


    }
}