using Microsoft.EntityFrameworkCore;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Payouts;

namespace PaySplit.Infrastructure.Persistence.Repositories
{
    public sealed class PayoutRepository : IPayoutRepository
    {
        private readonly AppDbContext _dbContext;

        public PayoutRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Payout?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Payouts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Payout payout, CancellationToken cancellationToken = default)
        {
            await _dbContext.Payouts.AddAsync(payout, cancellationToken);
        }
    }
}
