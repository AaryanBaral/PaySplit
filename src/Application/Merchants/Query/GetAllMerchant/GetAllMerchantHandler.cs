using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantHandler : IQueryHandler<GetAllMerchantQuery, Result<List<GetAllMerchantDto>>>
    {
        private readonly IApplicationDbContext _db;

        public GetAllMerchantHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Result<List<GetAllMerchantDto>>> HandleAsync(GetAllMerchantQuery query, CancellationToken cancellationToken = default)
        {
            var filter = query.PaginationFilter;

            var dbQuery = _db.Merchants
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLower();
                dbQuery = dbQuery.Where(m =>
                    m.Name.ToLower().Contains(search) ||
                    m.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                dbQuery = dbQuery.Where(m => m.Status.ToString() == filter.Status);
            }

            var skip = (filter.Page - 1) * filter.PageSize;

            var merchants = await dbQuery
                .OrderBy(m => m.CreatedAtUtc)
                .Skip(skip)
                .Take(filter.PageSize)
                .Select(m => new GetAllMerchantDto(
                    m.Id,
                    m.TenantId,
                    m.Name,
                    m.Email,
                    m.RevenueShare.Value,
                    m.Status.ToString(),
                    m.CreatedAtUtc))
                .ToListAsync(cancellationToken);

            return Result<List<GetAllMerchantDto>>.Success(merchants);
        }
    }
}
