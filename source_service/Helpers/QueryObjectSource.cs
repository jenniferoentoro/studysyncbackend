using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Helpers
{
    public class QueryObjectSource
    {
        public string? Title { get; set; } = null;

        public string? UserId { get; set; } = null;

        public string? SortBy { get; set; } = null;

        public bool IsDecsending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}