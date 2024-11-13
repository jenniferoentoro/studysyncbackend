using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Source
{
    public class UploadFile
    {
        public string FileName { get; set; } = string.Empty;

        public string UrlFile { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}