using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Dtos.Category;
using source_service.Dtos.User;
using source_service.Model;

namespace source_service.Dtos.Source
{
    public class SourceDto
    {
        public string Id { get; set; }
        public String Title { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public String UrlFile { get; set; } = string.Empty;

        public String FileName { get; set; } = string.Empty;

        public CategoryDto? Category { get; set; }
        public UserDto? User { get; set; }

        public string privacy { get; set; }


    }
}