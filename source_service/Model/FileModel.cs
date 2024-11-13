using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Model
{
    public class FileModel
    {
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}