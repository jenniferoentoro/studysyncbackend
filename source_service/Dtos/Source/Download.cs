using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Source
{
    public class Download
    {
        public Stream? File { get; set; }

        public string? ContentType { get; set; }
    }
}