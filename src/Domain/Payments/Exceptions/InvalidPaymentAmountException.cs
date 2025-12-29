using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Payments.Exceptions
{
    public sealed class InvalidPaymentAmountException : DomainException
    {
        public InvalidPaymentAmountException(decimal amount)
            : base($"Payment amount must be > 0. Received: {amount}.") { }
    }
}
