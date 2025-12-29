
using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
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
            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;

            RequestedByUserId = requestedByUserId;
            RequestedAtUtc = requestedAtUtc;

            Status = PayoutStatus.Requested;

            Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim();
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        }


        public static Result<Payout> Request(
    Guid tenantId,
    Guid merchantId,
    decimal amount,
    string currency,
    Guid requestedByUserId,
    DateTimeOffset requestedAtUtc,
    string? reference = null,
    string? notes = null)
        {
            if (tenantId == Guid.Empty)
                return Result<Payout>.Failure("Tenant id is required.");

            if (merchantId == Guid.Empty)
                return Result<Payout>.Failure("Merchant id is required.");

            if (requestedByUserId == Guid.Empty)
                return Result<Payout>.Failure("Requested by user id is required.");

            if (requestedAtUtc == default)
                return Result<Payout>.Failure("Requested time is required.");

            var moneyResult = Money.Create(currency, amount);
            if (!moneyResult.IsSuccess || moneyResult.Value is null)
                return Result<Payout>.Failure(moneyResult.Error ?? "Payout amount is invalid.");

            if (moneyResult.Value.Amount <= 0)
                return Result<Payout>.Failure(new PayoutAmountInvalidException(moneyResult.Value.Amount).Message);

            return Result<Payout>.Success(
                new Payout(
                    tenantId,
                    merchantId,
                    moneyResult.Value,
                    requestedByUserId,
                    requestedAtUtc,
                    reference,
                    notes));
        }

        public Result Approve(Guid approvedByUserId, DateTimeOffset approvedAtUtc)
        {
            if (Status != PayoutStatus.Requested)
                return Result.Failure(new PayoutInvalidStatusTransitionException("approve", Status).Message);

            if (approvedByUserId == Guid.Empty)
                return Result.Failure("Approved by user id is required.");

            if (approvedAtUtc == default)
                return Result.Failure("Approved time is required.");

            Status = PayoutStatus.Approved;
            ApprovedByUserId = approvedByUserId;
            ApprovedAtUtc = approvedAtUtc;
            return Result.Success();
        }

        public Result MarkCompleted(Guid completedByUserId, DateTimeOffset completedAtUtc, string? reference = null)
        {
            if (Status != PayoutStatus.Approved)
                return Result.Failure(new PayoutInvalidStatusTransitionException("complete", Status).Message);

            if (completedByUserId == Guid.Empty)
                return Result.Failure("Completed by user id is required.");

            if (completedAtUtc == default)
                return Result.Failure("Completed time is required.");

            Status = PayoutStatus.Completed;
            CompletedByUserId = completedByUserId;
            CompletedAtUtc = completedAtUtc;

            if (!string.IsNullOrWhiteSpace(reference))
            {
                Reference = reference.Trim();
            }
            return Result.Success();
        }

        public Result Reject(Guid rejectedByUserId, DateTimeOffset rejectedAtUtc, string? notes = null)
        {
            if (Status != PayoutStatus.Requested)
                return Result.Failure(new PayoutInvalidStatusTransitionException("reject", Status).Message);

            if (rejectedByUserId == Guid.Empty)
                return Result.Failure("Rejected by user id is required.");

            if (rejectedAtUtc == default)
                return Result.Failure("Rejected time is required.");

            Status = PayoutStatus.Rejected;
            RejectedByUserId = rejectedByUserId;
            RejectedAtUtc = rejectedAtUtc;

            if (!string.IsNullOrWhiteSpace(notes))
            {
                Notes = notes.Trim();
            }
            return Result.Success();
        }
    }
}
