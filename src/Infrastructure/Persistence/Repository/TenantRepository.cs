using Application.Interfaces.Repository;
using Domain.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _dbContext;

        public TenantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
        {
            await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        }
    }
}