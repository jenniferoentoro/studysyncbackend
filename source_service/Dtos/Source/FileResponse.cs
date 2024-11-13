using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Source
{
    public class FileResponse
    {
        public String FileName { get; set; } = string.Empty;
        public String FileType { get; set; } = string.Empty;
    }
}