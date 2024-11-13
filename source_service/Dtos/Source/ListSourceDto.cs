using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Dtos.Category;
using source_service.Dtos.User;
using source_service.Model;

namespace source_service.Dtos.Source
{
    public class ListSourceDto
    {
        public IEnumerable<SourceDto> listSource { get; set; } = new List<SourceDto>();
        public int Total { get; set; }


    }
}