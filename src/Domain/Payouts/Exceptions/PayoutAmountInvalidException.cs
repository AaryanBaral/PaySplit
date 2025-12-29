using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Payouts.Exceptions
{
    public sealed class PayoutAmountInvalidException : DomainException
    {
        public PayoutAmountInvalidException(decimal amount)
            : base($"Payout amount must be positive. Received: {amount}.") { }
    }
}
