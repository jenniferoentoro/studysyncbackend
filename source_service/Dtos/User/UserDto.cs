using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public String Email { get; set; } = string.Empty;

        public UserDto(string id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}