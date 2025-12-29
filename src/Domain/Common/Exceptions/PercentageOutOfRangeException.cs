namespace PaySplit.Domain.Common.Exceptions
{
    public sealed class PercentageOutOfRangeException : DomainException
    {
        public PercentageOutOfRangeException(decimal value)
            : base($"Percentage must be between 0 and 100 (exclusive). Received: {value}.") { }
    }
}
