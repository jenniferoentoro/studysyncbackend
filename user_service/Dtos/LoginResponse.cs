using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Dtos
{
    public class LoginResponse
    {

        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}