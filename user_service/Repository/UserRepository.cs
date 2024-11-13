using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using user_service.Data;
using user_service.model;
using user_service.Model;
using user_service.Repository.Interfaces;

namespace user_service.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly IMongoCollection<User> _users;

        public UserRepository(IOptions<UserDatabaseConfiguration> userConfiguration)
        {
            var mongoClient = new MongoClient(userConfiguration.Value.ConnectionString);

            var database = mongoClient.GetDatabase(userConfiguration.Value.DatabaseName);

            _users = database.GetCollection<User>(userConfiguration.Value.UsersCollectionName);
        }
        public User CreateUser(User user)
        { 
            _users.InsertOne(user);


            return user;
        }
        public void DeleteUser(string id)
        {
            var result = _users.DeleteOne(u => u.Id == id);
            if (result.DeletedCount == 0)
            {
                throw new ArgumentNullException("User not found");
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public User GetUserByEmail(string email)
        {
            return _users.Find(u => u.Email == email).FirstOrDefault();
        }

        public async Task<User> GetUser(string id)
        {
            var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;

        }

        public User UpdateUser(User user)
        {
            var result = _users.ReplaceOne(u => u.Id == user.Id, user);
            if (result.ModifiedCount == 0)
            {
                throw new ArgumentNullException("User not found");
            }
            return user;

        }
    }
}