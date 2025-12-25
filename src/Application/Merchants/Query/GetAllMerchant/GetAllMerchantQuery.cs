using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Filter;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Query.GetAllMerchant
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
