using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdQuery : IQuery<Result<GetMerchantByIdDto>>
    {
        public Guid Id { get; }

        public GetMerchantByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
