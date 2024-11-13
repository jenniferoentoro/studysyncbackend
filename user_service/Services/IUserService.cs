using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.model;

namespace user_service.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUser(string id);
        public User GetUserByEmail(string email);
        public User CreateUser(User user);
        public User UpdateUser(User user);
        public void DeleteUser(string id);

    }
}