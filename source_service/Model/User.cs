using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Email { get; set; } = string.Empty;

    }
}