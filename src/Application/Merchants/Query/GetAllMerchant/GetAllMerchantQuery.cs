using Application.Common.Abstractions;
using Application.Common.Filter;
using Application.Common.Results;

namespace Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantQuery : IQuery<Result<List<GetAllMerchantDto>>>
    {
        public PaginationFilter PaginationFilter { get; }

        public GetAllMerchantQuery(PaginationFilter paginationFilter)
        {
            PaginationFilter = paginationFilter;
        }
    }
}
