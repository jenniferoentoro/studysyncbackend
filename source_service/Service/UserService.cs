using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Service.Interface;

namespace source_service.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetUser(id);
        }
    }
}