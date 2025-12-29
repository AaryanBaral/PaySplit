namespace PaySplit.Domain.Common.Exceptions
{
    public sealed class MoneyAmountNegativeException : DomainException
    {
        public MoneyAmountNegativeException(decimal amount)
            : base($"Amount cannot be negative. Received: {amount}.") { }
    }
}
