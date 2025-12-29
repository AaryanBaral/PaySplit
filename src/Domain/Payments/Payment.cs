
using PaySplit.Domain.Common;
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
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant id is required.", nameof(tenantId));

            if (merchantId == Guid.Empty)
                throw new ArgumentException("Merchant id is required.", nameof(merchantId));

            // validations and setter code for this class
            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Amount <= 0)
                throw new InvalidPaymentAmountException(amount.Amount);

            if (string.IsNullOrWhiteSpace(externalPaymentId))
                throw new ArgumentException("External payment id is required.", nameof(externalPaymentId));

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
        public static Payment CreatePending(Guid tenantId,
        Guid merchantId,
        decimal amount,
        string currency,
        string externalPaymentId)
        {
            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new InvalidCurrencyException(currency);

            if (amount <= 0)
                throw new InvalidPaymentAmountException(amount);

            var money = Money.Create(currency, amount);
            return new Payment(tenantId, merchantId, money, externalPaymentId);

        }

        // mark the payment success after buisness processing.
        public void MarkSucceeded(DateTimeOffset completedAtUtc)
        {
            if (Status != PaymentStatus.Pending)
                throw new PaymentInvalidStatusTransitionException("mark as succeeded", Status);

            if (completedAtUtc == default)
                throw new ArgumentException("Completed time is required.", nameof(completedAtUtc));

            Status = PaymentStatus.Succeeded;
            CompletedAtUtc = completedAtUtc;
        }

        // Mark the payment as failed (after gateway callback)
        public void MarkFailed(DateTimeOffset completedAtUtc)
        {
            if (Status != PaymentStatus.Pending)
                throw new PaymentInvalidStatusTransitionException("mark as failed", Status);

            if (completedAtUtc == default)
                throw new ArgumentException("Completed time is required.", nameof(completedAtUtc));

            Status = PaymentStatus.Failed;
            CompletedAtUtc = completedAtUtc;
        }

        public (Money merchantAmount, Money tenantAmount) CalculateRevenueSplit(Percentage merchantShare)
        {
            if (merchantShare is null)
                throw new ArgumentNullException(nameof(merchantShare));
            if (Status != PaymentStatus.Succeeded)
                throw new PaymentInvalidStatusTransitionException("split revenue", Status);

            // multiply the amount by the fraction of the merchant share
            var merchantAmount = Amount.Multiply(merchantShare.AsFraction());

            var tenantAmount = Amount.Subtract(merchantAmount);
            return (merchantAmount, tenantAmount);
        }


    }
}
