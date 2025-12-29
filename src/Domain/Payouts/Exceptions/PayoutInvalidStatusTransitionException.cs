using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Payouts.Exceptions
{
    public sealed class PayoutInvalidStatusTransitionException : DomainException
    {
        public PayoutInvalidStatusTransitionException(string action, PayoutStatus status)
            : base($"Cannot {action} when payout status is {status}.") { }
    }
}
