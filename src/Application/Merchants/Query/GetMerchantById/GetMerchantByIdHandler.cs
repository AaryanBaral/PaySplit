using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdHandler: IRequestHandler<GetMerchantByIdQuery, Result<GetMerchantByIdDto>>
    {
        private readonly IMerchantRepository _repository;

        public GetMerchantByIdHandler(IMerchantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetMerchantByIdDto>> HandleAsync(GetMerchantByIdQuery query, CancellationToken cancellationToken = default)
        {
            if (query.Id == Guid.Empty)
            {
                return Result<GetMerchantByIdDto>.Failure("Merchant id is required.");
            }

            var merchant = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (merchant is null)
            {
                return Result<GetMerchantByIdDto>.Failure("Merchant not found");
            }

            return Result<GetMerchantByIdDto>.Success(merchant.ToGetMerchantByIdDto());
        }
    
        public Task<Result<GetMerchantByIdDto>> Handle(GetMerchantByIdQuery request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
