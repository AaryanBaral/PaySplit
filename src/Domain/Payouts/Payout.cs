
using PaySplit.Domain.Common;
using PaySplit.Domain.Payouts.Exceptions;

namespace PaySplit.Domain.Payouts
{
    public class Payout : Entity
    {
        public Guid TenantId { get; private set; }
        public Guid MerchantId { get; private set; }
        public Money Amount { get; private set; } = default!;
        public PayoutStatus Status { get; private set; }

        public DateTimeOffset RequestedAtUtc { get; private set; }
        public Guid RequestedByUserId { get; private set; }

        public DateTimeOffset? ApprovedAtUtc { get; private set; }
        public Guid? ApprovedByUserId { get; private set; }

        public DateTimeOffset? CompletedAtUtc { get; private set; }
        public Guid? CompletedByUserId { get; private set; }

        public DateTimeOffset? RejectedAtUtc { get; private set; }
        public Guid? RejectedByUserId { get; private set; }

        public string? Reference { get; private set; }
        public string? Notes { get; private set; }

        private Payout() { }

        private Payout(
            Guid tenantId,
            Guid merchantId,
            Money amount,
            Guid requestedByUserId,
            DateTimeOffset requestedAtUtc,
            string? reference,
            string? notes)
            : base()
        {
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant id is required.", nameof(tenantId));

            if (merchantId == Guid.Empty)
                throw new ArgumentException("Merchant id is required.", nameof(merchantId));

            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Amount <= 0)
                throw new PayoutAmountInvalidException(amount.Amount);

            if (requestedByUserId == Guid.Empty)
                throw new ArgumentException("Requested by user id is required.", nameof(requestedByUserId));

            if (requestedAtUtc == default)
                throw new ArgumentException("Requested time is required.", nameof(requestedAtUtc));

            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;

            RequestedByUserId = requestedByUserId;
            RequestedAtUtc = requestedAtUtc;

            Status = PayoutStatus.Requested;

            Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim();
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        }


        public static Payout Request(
    Guid tenantId,
    Guid merchantId,
    decimal amount,
    string currency,
    Guid requestedByUserId,
    DateTimeOffset requestedAtUtc,
    string? reference = null,
    string? notes = null)
        {
            var money = Money.Create(currency, amount);
            return new Payout(
                tenantId,
                merchantId,
                money,
                requestedByUserId,
                requestedAtUtc,
                reference,
                notes);
        }

        public void Approve(Guid approvedByUserId, DateTimeOffset approvedAtUtc)
        {
            if (Status != PayoutStatus.Requested)
                throw new PayoutInvalidStatusTransitionException("approve", Status);

            if (approvedByUserId == Guid.Empty)
                throw new ArgumentException("Approved by user id is required.", nameof(approvedByUserId));

            if (approvedAtUtc == default)
                throw new ArgumentException("Approved time is required.", nameof(approvedAtUtc));

            Status = PayoutStatus.Approved;
            ApprovedByUserId = approvedByUserId;
            ApprovedAtUtc = approvedAtUtc;
        }

        public void MarkCompleted(Guid completedByUserId, DateTimeOffset completedAtUtc, string? reference = null)
        {
            if (Status != PayoutStatus.Approved)
                throw new PayoutInvalidStatusTransitionException("complete", Status);

            if (completedByUserId == Guid.Empty)
                throw new ArgumentException("Completed by user id is required.", nameof(completedByUserId));

            if (completedAtUtc == default)
                throw new ArgumentException("Completed time is required.", nameof(completedAtUtc));

            Status = PayoutStatus.Completed;
            CompletedByUserId = completedByUserId;
            CompletedAtUtc = completedAtUtc;

            if (!string.IsNullOrWhiteSpace(reference))
            {
                Reference = reference.Trim();
            }
        }

        public void Reject(Guid rejectedByUserId, DateTimeOffset rejectedAtUtc, string? notes = null)
        {
            if (Status != PayoutStatus.Requested)
                throw new PayoutInvalidStatusTransitionException("reject", Status);

            if (rejectedByUserId == Guid.Empty)
                throw new ArgumentException("Rejected by user id is required.", nameof(rejectedByUserId));

            if (rejectedAtUtc == default)
                throw new ArgumentException("Rejected time is required.", nameof(rejectedAtUtc));

            Status = PayoutStatus.Rejected;
            RejectedByUserId = rejectedByUserId;
            RejectedAtUtc = rejectedAtUtc;

            if (!string.IsNullOrWhiteSpace(notes))
            {
                Notes = notes.Trim();
            }
        }
    }
}
