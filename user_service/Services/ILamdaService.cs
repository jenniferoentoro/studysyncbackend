using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.model;
namespace user_service.Services
{
    public interface ILambdaService
    {
        Task<List<User>> GetUsersFromLambda();
    }
}