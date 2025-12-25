using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdHandler : IQueryHandler<GetMerchantByIdQuery, Result<GetMerchantByIdDto>>
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

            var dto = new GetMerchantByIdDto(
                merchant.Id,
                merchant.TenantId,
                merchant.Name,
                merchant.Email,
                merchant.RevenueShare.Value,
                merchant.Status.ToString(),
                merchant.CreatedAtUtc,
                merchant.DeactivatedAtUtc,
                merchant.SuspendedAtUtc);

            return Result<GetMerchantByIdDto>.Success(dto);
        }
    }
}
