using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.model;
using user_service.Repository.Interfaces;

namespace user_service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User CreateUser(User user)
        {
            return _userRepository.CreateUser(user);

        }

        public void DeleteUser(string id)
        {
            _userRepository.DeleteUser(id);
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public async Task<User> GetUser(string id)
        {
            return await _userRepository.GetUser(id);
        }
        public User UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }
    }
}