using PaySplit.Domain.Ledgers;

namespace PaySplit.Application.Interfaces.Repository
{
    public interface ILedgerEntryRepository
    {
        Task AddAsync(LedgerEntry entry, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<LedgerEntry>> GetByMerchantAsync(
        Guid tenantId,
        Guid merchantId,
        CancellationToken cancellationToken = default);
    }
}
