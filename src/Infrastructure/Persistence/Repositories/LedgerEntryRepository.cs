using Microsoft.EntityFrameworkCore;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Ledgers;

namespace PaySplit.Infrastructure.Persistence.Repositories
{
    public sealed class LedgerEntryRepository : ILedgerEntryRepository
    {
        private readonly AppDbContext _dbContext;

        public LedgerEntryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(LedgerEntry entry, CancellationToken cancellationToken = default)
        {
            await _dbContext.LedgerEntries.AddAsync(entry, cancellationToken);
        }

        public async Task<IReadOnlyList<LedgerEntry>> GetByMerchantAsync(
            Guid tenantId,
            Guid merchantId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.LedgerEntries
                .AsNoTracking()
                .Where(e => e.TenantId == tenantId && e.MerchantId == merchantId)
                .OrderByDescending(e => e.OccurredAtUtc)
                .ToListAsync(cancellationToken);
        }
    }
}
