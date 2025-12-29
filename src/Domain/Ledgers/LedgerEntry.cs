

using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
using PaySplit.Domain.Ledgers.Exceptions;

namespace PaySplit.Domain.Ledgers
{
    public class LedgerEntry : Entity
    {
        public Guid TenantId { get; private set; }

        // Null for tenant-level entries (e.g., tenant revenue),
        // set for merchant entries (e.g., merchant earnings, merchant payouts).
        public Guid? MerchantId { get; private set; }

        public Money Amount { get; private set; } = default!;
        public LedgerEntryKind Kind { get; private set; }
        public LedgerEntrySourceType SourceType { get; private set; }
        public Guid SourceId { get; private set; }          // e.g., Payment.Id, Payout.Id
        public string Description { get; private set; } = default!;
        public DateTimeOffset OccurredAtUtc { get; private set; }

        private LedgerEntry()
        {
        }
        private LedgerEntry(Guid tenantId,
        Guid? merchantId,
        Money amount,
        LedgerEntryKind kind,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc) : base()
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;
            Kind = kind;
            SourceType = sourceType;
            SourceId = sourceId;
            Description = description.Trim();
            OccurredAtUtc = occurredAtUtc;
        }
        public static Result<LedgerEntry> CreateMerchantCredit(
    Guid tenantId,
    Guid merchantId,
    Money amount,
    LedgerEntrySourceType sourceType,
    Guid sourceId,
    string description,
    DateTimeOffset occurredAtUtc)
        {
            return Create(
                tenantId,
                merchantId,
                amount,
                LedgerEntryKind.MerchantCredit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }
        public static Result<LedgerEntry> CreateMerchantDebit(
        Guid tenantId,
        Guid merchantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return Create(
                tenantId,
                merchantId,
                amount,
                LedgerEntryKind.MerchantDebit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }

        public static Result<LedgerEntry> CreateTenantCredit(
        Guid tenantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return Create(
                tenantId,
                merchantId: null,
                amount,
                LedgerEntryKind.TenantCredit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }
        public static Result<LedgerEntry> CreateTenantDebit(
        Guid tenantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return Create(
                tenantId,
                merchantId: null,
                amount,
                LedgerEntryKind.TenantDebit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }

        private static Result<LedgerEntry> Create(
            Guid tenantId,
            Guid? merchantId,
            Money amount,
            LedgerEntryKind kind,
            LedgerEntrySourceType sourceType,
            Guid sourceId,
            string description,
            DateTimeOffset occurredAtUtc)
        {
            if (tenantId == Guid.Empty)
                return Result<LedgerEntry>.Failure("Tenant id is required.");

            if (amount is null)
                return Result<LedgerEntry>.Failure("Ledger amount is required.");

            if (amount.Amount <= 0)
                return Result<LedgerEntry>.Failure(new LedgerAmountInvalidException(amount.Amount).Message);

            if (sourceId == Guid.Empty)
                return Result<LedgerEntry>.Failure("Source id is required.");

            if (string.IsNullOrWhiteSpace(description))
                return Result<LedgerEntry>.Failure("Description is required.");

            if (occurredAtUtc == default)
                return Result<LedgerEntry>.Failure("Occurred time is required.");

            var isMerchantKind = kind == LedgerEntryKind.MerchantCredit || kind == LedgerEntryKind.MerchantDebit;
            if (isMerchantKind && merchantId is null)
                return Result<LedgerEntry>.Failure("Merchant id is required for merchant ledger entries.");

            if (!isMerchantKind && merchantId is not null)
                return Result<LedgerEntry>.Failure("Merchant id must be null for tenant ledger entries.");

            return Result<LedgerEntry>.Success(
                new LedgerEntry(
                    tenantId,
                    merchantId,
                    amount,
                    kind,
                    sourceType,
                    sourceId,
                    description,
                    occurredAtUtc));
        }
    }
}
