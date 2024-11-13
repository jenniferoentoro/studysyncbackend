using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Dtos.Error
{
    public class CustomErrorResponse
    {
        public string Type { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public string TraceId { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
    }
}