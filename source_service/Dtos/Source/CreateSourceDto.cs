using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Source
{
    public class CreateSourceDto
    {
        [Required]
        [MaxLength(100)]
        public String Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public String Description { get; set; } = string.Empty;

        [Required]
        public string CategoryId { get; set; }
        // [Required]
        // public string UserId { get; set; }

        [Required]
        public IFormFile? File { get; set; }

        [Required]
        public string privacy { get; set; }
    }
}