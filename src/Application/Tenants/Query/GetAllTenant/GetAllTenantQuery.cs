using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Abstractions;
using Application.Common.Filter;
using Application.Common.Results;

namespace Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantQuery : IQuery<Result<List<GetAllTenantDto>>>
    {
        public PaginationFilter PaginationFilter { get; }
        public GetAllTenantQuery(PaginationFilter paginationFilter)
        {
            PaginationFilter = paginationFilter;
        }

    }
}