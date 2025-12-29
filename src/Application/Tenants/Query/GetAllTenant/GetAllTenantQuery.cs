using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

using PaySplit.Application.Common.Filter;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantQuery : IRequest<Result<List<GetAllTenantDto>>>
    {
        public PaginationFilter PaginationFilter { get; }
        public GetAllTenantQuery(PaginationFilter paginationFilter)
        {
            PaginationFilter = paginationFilter;
        }

    }
}
