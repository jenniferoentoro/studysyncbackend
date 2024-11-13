using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace source_service.Dtos.Source
{
    public class DownloadResult
    {
        public Stream FileContent { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
    }
}