using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public String Name { get; set; } = string.Empty;
    }
}