using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Model;

namespace source_service.Service.Interface
{
    public interface IUserService
    {
        public Task<User> GetUser(int id);
    }
}