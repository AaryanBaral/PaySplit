using PaySplit.Application.Common.Filter;
using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantQuery : IRequest<Result<List<GetAllMerchantDto>>>
    {
        public PaginationFilter PaginationFilter { get; }

        public GetAllMerchantQuery(PaginationFilter paginationFilter)
        {
            PaginationFilter = paginationFilter;
        }
    }
}
