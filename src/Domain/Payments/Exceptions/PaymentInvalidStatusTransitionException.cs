using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Payments.Exceptions
{
    public sealed class PaymentInvalidStatusTransitionException : DomainException
    {
        public PaymentInvalidStatusTransitionException(string action, PaymentStatus status)
            : base($"Cannot {action} when payment status is {status}.") { }
    }
}
