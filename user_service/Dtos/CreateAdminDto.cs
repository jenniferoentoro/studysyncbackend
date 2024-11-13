using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Dtos
{
    public class CreateAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; } = null!;


    }
}