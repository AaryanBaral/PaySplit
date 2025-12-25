using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PaySplit.Domain.Ledgers;

namespace PaySplit.Application.Repository
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