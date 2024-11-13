using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.Model;

namespace user_service.Dtos
{
    public class UserDto
    {

        public String? Id { get; set; }

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Grade { get; set; }

        public string School { get; set; } = null!;
        public string Role { get; set; }
    }
}