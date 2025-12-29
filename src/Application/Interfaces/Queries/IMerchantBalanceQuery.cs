using PaySplit.Application.Common.Models;

namespace PaySplit.Application.Interfaces.Queries;

public interface IMerchantBalanceQuery
{
    Task<MerchantBalanceDto> GetAsync(
        Guid tenantId,
        Guid merchantId,
        CancellationToken ct = default);
}

