using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Source
{
    public class SourceResponseDto
    {
        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<SourceDto> Sources { get; set; } = new List<SourceDto>();
    }
}