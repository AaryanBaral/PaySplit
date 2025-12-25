using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdHandler : IQueryHandler<GetMerchantByIdQuery, Result<GetMerchantByIdDto>>
    {
        private readonly IApplicationDbContext _db;

        public GetMerchantByIdHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Result<GetMerchantByIdDto>> HandleAsync(GetMerchantByIdQuery query, CancellationToken cancellationToken = default)
        {
            if (query.Id == Guid.Empty)
            {
                return Result<GetMerchantByIdDto>.Failure("Merchant id is required.");
            }

            var merchant = await _db.Merchants
                .AsNoTracking()
                .Where(m => m.Id == query.Id)
                .Select(m => new GetMerchantByIdDto(
                    m.Id,
                    m.TenantId,
                    m.Name,
                    m.Email,
                    m.RevenueShare.Value,
                    m.Status.ToString(),
                    m.CreatedAtUtc,
                    m.DeactivatedAtUtc,
                    m.SuspendedAtUtc))
                .FirstOrDefaultAsync(cancellationToken);

            if (merchant is null)
            {
                return Result<GetMerchantByIdDto>.Failure("Merchant not found");
            }

            return Result<GetMerchantByIdDto>.Success(merchant);
        }
    }
}
