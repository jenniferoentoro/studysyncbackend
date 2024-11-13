using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace source_service.Dtos.Category
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;


    }
}