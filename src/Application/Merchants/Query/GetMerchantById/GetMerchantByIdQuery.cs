using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Merchants.Query.GetMerchantById
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
