using PaySplit.Domain.Common.Exceptions;

namespace PaySplit.Domain.Payments.Exceptions
{
    public sealed class InvalidCurrencyException : DomainException
    {
        public InvalidCurrencyException(string currency)
            : base($"Invalid currency: '{currency}'.") { }
    }
}
