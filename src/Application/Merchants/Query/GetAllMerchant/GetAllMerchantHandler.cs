using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantHandler : IQueryHandler<GetAllMerchantQuery, Result<List<GetAllMerchantDto>>>
    {
        private readonly IMerchantRepository _repository;

        public GetAllMerchantHandler(IMerchantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetAllMerchantDto>>> HandleAsync(GetAllMerchantQuery query, CancellationToken cancellationToken = default)
        {
            var filter = query.PaginationFilter;

            var merchants = await _repository.GetAllAsync(filter, cancellationToken);

            var result = merchants
                .Select(m => new GetAllMerchantDto(
                    m.Id,
                    m.TenantId,
                    m.Name,
                    m.Email,
                    m.RevenueShare.Value,
                    m.Status.ToString(),
                    m.CreatedAtUtc))
                .ToList();

            return Result<List<GetAllMerchantDto>>.Success(result);
        }
    }
}
