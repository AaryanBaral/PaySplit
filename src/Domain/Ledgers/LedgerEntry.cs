

using PaySplit.Domain.Common;

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
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant id is required.", nameof(tenantId));

            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Amount <= 0)
                throw new ArgumentException("Ledger amount must be positive.", nameof(amount));

            if (sourceId == Guid.Empty)
                throw new ArgumentException("Source id is required.", nameof(sourceId));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));

            var isMerchantKind = kind == LedgerEntryKind.MerchantCredit || kind == LedgerEntryKind.MerchantDebit;
            if (isMerchantKind && merchantId is null)
                throw new ArgumentException("Merchant id is required for merchant ledger entries.", nameof(merchantId));

            if (!isMerchantKind && merchantId is not null)
                throw new ArgumentException("Merchant id must be null for tenant ledger entries.", nameof(merchantId));

            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;
            Kind = kind;
            SourceType = sourceType;
            SourceId = sourceId;
            Description = description.Trim();
            OccurredAtUtc = occurredAtUtc;
        }
        public static LedgerEntry CreateMerchantCredit(
    Guid tenantId,
    Guid merchantId,
    Money amount,
    LedgerEntrySourceType sourceType,
    Guid sourceId,
    string description,
    DateTimeOffset occurredAtUtc)
        {
            return new LedgerEntry(
                tenantId,
                merchantId,
                amount,
                LedgerEntryKind.MerchantCredit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }
        public static LedgerEntry CreateMerchantDebit(
        Guid tenantId,
        Guid merchantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return new LedgerEntry(
                tenantId,
                merchantId,
                amount,
                LedgerEntryKind.MerchantDebit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }

        public static LedgerEntry CreateTenantCredit(
        Guid tenantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return new LedgerEntry(
                tenantId,
                merchantId: null,
                amount,
                LedgerEntryKind.TenantCredit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }
        public static LedgerEntry CreateTenantDebit(
        Guid tenantId,
        Money amount,
        LedgerEntrySourceType sourceType,
        Guid sourceId,
        string description,
        DateTimeOffset occurredAtUtc)
        {
            return new LedgerEntry(
                tenantId,
                merchantId: null,
                amount,
                LedgerEntryKind.TenantDebit,
                sourceType,
                sourceId,
                description,
                occurredAtUtc);
        }
    }
}
