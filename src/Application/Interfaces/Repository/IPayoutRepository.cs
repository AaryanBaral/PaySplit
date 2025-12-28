

using PaySplit.Domain.Payouts;

namespace PaySplit.Application.Repository
{
    public interface IPayoutRepository
    {
        Task AddAsync(Payout payout, CancellationToken cancellationToken = default);
        Task<Payout?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}