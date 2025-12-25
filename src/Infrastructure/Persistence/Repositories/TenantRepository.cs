using Microsoft.EntityFrameworkCore;

using PaySplit.Application.Common.Filter;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;

namespace PaySplit.Infrastructure.Persistence.Repositories
{
    public sealed class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _dbContext;

        public TenantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
        {
            await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        }

        public async Task<List<Tenant>> GetAllAsync(PaginationFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Tenants
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLower();
                query = query.Where(t => t.Name.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(t => t.Status.ToString() == filter.Status);
            }

            var skip = (filter.Page - 1) * filter.PageSize;

            return await query
                .OrderBy(t => t.CreatedAtUtc)
                .Skip(skip)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
