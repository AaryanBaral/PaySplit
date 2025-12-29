using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdQuery : IRequest<Result<GetMerchantByIdDto>>
    {
        public Guid Id { get; }

        public GetMerchantByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
