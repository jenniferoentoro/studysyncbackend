using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Dtos
{
    public class UpdateUserDto
    {
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; } = null!;

        public string Grade { get; set; } = null!;

        public string School { get; set; } = null!;


    }
}