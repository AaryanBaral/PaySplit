namespace PaySplit.Domain.Common.Exceptions
{
    public sealed class MoneyResultNegativeException : DomainException
    {
        public MoneyResultNegativeException()
            : base("Resulting money cannot be negative.") { }
    }
}
