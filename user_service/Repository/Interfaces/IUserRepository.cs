using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.model;

namespace user_service.Repository.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        Task<User> GetUser(string id);
        User CreateUser(User user);
        User UpdateUser(User user);
        void DeleteUser(string id);

        Task<IEnumerable<User>> GetAllUsers();

    }
}