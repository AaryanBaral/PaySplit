
using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
using PaySplit.Domain.Payments.Exceptions;

namespace PaySplit.Domain.Payments
{
    public class Payment : Entity
    {
        public Guid TenantId { get; private set; }
        public Guid MerchantId { get; private set; }
        public Money Amount { get; private set; } = default!;
        public PaymentStatus Status { get; private set; }
        public string ExternalPaymentId { get; private set; } = default!;
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? CompletedAtUtc { get; private set; }
        private Payment() { }
        private Payment(Guid tenantId,
        Guid merchantId,
        Money amount,
        string externalPaymentId) : base()
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;
            ExternalPaymentId = externalPaymentId.Trim();
            Status = PaymentStatus.Pending;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CompletedAtUtc = null;
        }

        // public function that is to be called my other class
        // this methods internally calls the constructor and retuern it.
        public static Result<Payment> CreatePending(
            Guid tenantId,
            Guid merchantId,
            decimal amount,
            string currency)
        {
            if (tenantId == Guid.Empty)
                return Result<Payment>.Failure("Tenant id is required.");

            if (merchantId == Guid.Empty)
                return Result<Payment>.Failure("Merchant id is required.");

            return CreateInternal(tenantId, merchantId, amount, currency, externalPaymentId: null);

        }

        public static Result<Payment> CreateIncoming(
            Guid tenantId,
            Guid merchantId,
            decimal amount,
            string currency,
            string externalPaymentId)
        {
            if (string.IsNullOrWhiteSpace(externalPaymentId))
                return Result<Payment>.Failure("External payment id is required.");

            return CreateInternal(tenantId, merchantId, amount, currency, externalPaymentId);
        }

        private static Result<Payment> CreateInternal(
            Guid tenantId,
            Guid merchantId,
            decimal amount,
            string currency,
            string? externalPaymentId)
        {
            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                return Result<Payment>.Failure(new InvalidCurrencyException(currency).Message);

            if (amount <= 0)
                return Result<Payment>.Failure(new InvalidPaymentAmountException(amount).Message);

            var moneyResult = Money.Create(currency, amount);
            if (!moneyResult.IsSuccess || moneyResult.Value is null)
                return Result<Payment>.Failure(moneyResult.Error ?? "Payment amount is invalid.");

            var externalIdValue = string.IsNullOrWhiteSpace(externalPaymentId) ? string.Empty : externalPaymentId.Trim();
            return Result<Payment>.Success(
                new Payment(tenantId, merchantId, moneyResult.Value, externalIdValue));
        }

        // mark the payment success after buisness processing.
        public Result MarkSucceeded(DateTimeOffset completedAtUtc)
        {
            if (Status != PaymentStatus.Pending)
                return Result.Failure(new PaymentInvalidStatusTransitionException("mark as succeeded", Status).Message);

            if (completedAtUtc == default)
                return Result.Failure("Completed time is required.");

            Status = PaymentStatus.Succeeded;
            CompletedAtUtc = completedAtUtc;
            return Result.Success();
        }

        // Mark the payment as failed (after gateway callback)
        public Result MarkFailed(DateTimeOffset completedAtUtc)
        {
            if (Status != PaymentStatus.Pending)
                return Result.Failure(new PaymentInvalidStatusTransitionException("mark as failed", Status).Message);

            if (completedAtUtc == default)
                return Result.Failure("Completed time is required.");

            Status = PaymentStatus.Failed;
            CompletedAtUtc = completedAtUtc;
            return Result.Success();
        }

        public Result<(Money merchantAmount, Money tenantAmount)> CalculateRevenueSplit(Percentage merchantShare)
        {
            if (merchantShare is null)
                throw new ArgumentNullException(nameof(merchantShare));
            if (Status != PaymentStatus.Succeeded)
                return Result<(Money merchantAmount, Money tenantAmount)>.Failure(
                    new PaymentInvalidStatusTransitionException("split revenue", Status).Message);

            // multiply the amount by the fraction of the merchant share
            var merchantAmount = Amount.Multiply(merchantShare.AsFraction());

            var tenantAmount = Amount.Subtract(merchantAmount);
            return Result<(Money merchantAmount, Money tenantAmount)>.Success((merchantAmount, tenantAmount));
        }


    }
}
