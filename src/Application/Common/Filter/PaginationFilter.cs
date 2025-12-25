using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaySplit.Application.Common.Filter
{
    public class PaginationFilter
    {
        public string? Search { get; set; }      // search by name
        public string? Status { get; set; }      // "Active", "Inactive", etc.
        public int Page { get; set; } = 1;       // which page
        public int PageSize { get; set; } = 20;  // items per page
    }
}
