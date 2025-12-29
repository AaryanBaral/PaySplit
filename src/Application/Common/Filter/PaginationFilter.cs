using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaySplit.Application.Common.Filter
{
    public sealed record PaginationFilter
    {
        public string? Search { get; init; }      // search by name
        public string? Status { get; init; }      // "Active", "Inactive", etc.
        public int Page { get; init; } = 1;       // which page
        public int PageSize { get; init; } = 20;  // items per page
    }
}
