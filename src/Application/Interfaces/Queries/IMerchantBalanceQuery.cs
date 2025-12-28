using PaySplit.Domain.Common;

namespace PaySplit.Application.Interfaces.Queries;

public interface IMerchantBalanceQuery
{
    Task<MerchantBalance> GetAsync(
        Guid tenantId,
        Guid merchantId,
        CancellationToken ct = default);
}

public sealed record MerchantBalance(
    Money Available,
    Money Posted,  
    Money Pending); 
