using PaySplit.Application.Common.Models;

using Microsoft.EntityFrameworkCore;
using PaySplit.Application.Interfaces.Queries;
using PaySplit.Domain.Common;
using PaySplit.Domain.Ledgers;
using PaySplit.Domain.Payouts;

namespace PaySplit.Infrastructure.Persistence.Queries;

public sealed class MerchantBalanceQuery : IMerchantBalanceQuery
{
    private readonly AppDbContext _dbContext;

    public MerchantBalanceQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MerchantBalanceDto> GetAsync(
        Guid tenantId,
        Guid merchantId,
        CancellationToken ct = default)
    {
        // 1) Tenant currency is the single source of truth
        var tenantCurrency = await _dbContext.Tenants
            .AsNoTracking()
            .Where(t => t.Id == tenantId)
            .Select(t => t.DefaultCurrency)
            .SingleOrDefaultAsync(ct);

        if (string.IsNullOrWhiteSpace(tenantCurrency))
            throw new InvalidOperationException($"Tenant '{tenantId}' not found or default currency missing.");

        tenantCurrency = tenantCurrency.Trim().ToUpperInvariant();

        // 2) ENFORCE: ledger currency must match tenant default currency
        var hasLedgerCurrencyMismatch = await _dbContext.LedgerEntries
            .AsNoTracking()
            .AnyAsync(e => e.TenantId == tenantId
                        && e.MerchantId == merchantId
                        && e.Amount.Currency != tenantCurrency, ct);

        if (hasLedgerCurrencyMismatch)
            throw new InvalidOperationException(
                $"Currency mismatch: ledger entries for merchant '{merchantId}' do not match tenant default currency '{tenantCurrency}'.");

        // 3) ENFORCE: payout currency must match tenant default currency
        var hasPayoutCurrencyMismatch = await _dbContext.Payouts
            .AsNoTracking()
            .AnyAsync(p => p.TenantId == tenantId
                        && p.MerchantId == merchantId
                        && p.Amount.Currency != tenantCurrency, ct);

        if (hasPayoutCurrencyMismatch)
            throw new InvalidOperationException(
                $"Currency mismatch: payouts for merchant '{merchantId}' do not match tenant default currency '{tenantCurrency}'.");

        // 4) Compute POSTED from ledger (credits - debits)
        // Since we enforced currency match above, we don't need currency filters here.
        var creditSum = await _dbContext.LedgerEntries
            .AsNoTracking()
            .Where(e => e.TenantId == tenantId
                     && e.MerchantId == merchantId
                     && e.Kind == LedgerEntryKind.MerchantCredit)
            .SumAsync(e => (decimal?)e.Amount.Amount, ct) ?? 0m;

        var debitSum = await _dbContext.LedgerEntries
            .AsNoTracking()
            .Where(e => e.TenantId == tenantId
                     && e.MerchantId == merchantId
                     && e.Kind == LedgerEntryKind.MerchantDebit)
            .SumAsync(e => (decimal?)e.Amount.Amount, ct) ?? 0m;

        var postedValue = creditSum - debitSum;
        if (postedValue < 0m) postedValue = 0m; // or throw if you want strict correctness

        // 5) Compute PENDING/RESERVED from payout requests
        var pendingValue = await _dbContext.Payouts
            .AsNoTracking()
            .Where(p => p.TenantId == tenantId
                     && p.MerchantId == merchantId
                     && (p.Status == PayoutStatus.Requested || p.Status == PayoutStatus.Approved))
            .SumAsync(p => (decimal?)p.Amount.Amount, ct) ?? 0m;

        if (pendingValue < 0m) pendingValue = 0m;

        // 6) Available = posted - pending
        var availableValue = postedValue - pendingValue;
        if (availableValue < 0m) availableValue = 0m;

        // IMPORTANT: Ensure Money.Create signature matches your Money type.
        // Most common is Money.Create(decimal amount, string currency)
        var postedMoney = Money.Create(tenantCurrency, postedValue);
        var pendingMoney = Money.Create(tenantCurrency, pendingValue);
        var availableMoney = Money.Create(tenantCurrency, availableValue);

        if (!postedMoney.IsSuccess || postedMoney.Value is null)
            throw new InvalidOperationException(postedMoney.Error ?? "Posted amount is invalid.");
        if (!pendingMoney.IsSuccess || pendingMoney.Value is null)
            throw new InvalidOperationException(pendingMoney.Error ?? "Pending amount is invalid.");
        if (!availableMoney.IsSuccess || availableMoney.Value is null)
            throw new InvalidOperationException(availableMoney.Error ?? "Available amount is invalid.");

        return new MerchantBalanceDto(
            Posted: postedMoney.Value,
            Pending: pendingMoney.Value,
            Available: availableMoney.Value);
    }
}
