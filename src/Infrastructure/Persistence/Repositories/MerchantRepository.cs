using Microsoft.EntityFrameworkCore;

using PaySplit.Application.Common.Filter;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Merchants;

namespace PaySplit.Infrastructure.Persistence.Repositories
{
    public sealed class MerchantRepository : IMerchantRepository
    {
        private readonly AppDbContext _dbContext;

        public MerchantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Merchants
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task AddAsync(Merchant merchant, CancellationToken cancellationToken = default)
        {
            await _dbContext.Merchants.AddAsync(merchant, cancellationToken);
        }

        public async Task<List<Merchant>> GetAllAsync(PaginationFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Merchants
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLower();
                query = query.Where(m =>
                    m.Name.ToLower().Contains(search) ||
                    m.Email.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(m => m.Status.ToString() == filter.Status);
            }

            var skip = (filter.Page - 1) * filter.PageSize;

            return await query
                .OrderBy(m => m.CreatedAtUtc)
                .Skip(skip)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
