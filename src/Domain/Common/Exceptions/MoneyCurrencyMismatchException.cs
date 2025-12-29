namespace PaySplit.Domain.Common.Exceptions
{
    public sealed class MoneyCurrencyMismatchException : DomainException
    {
        public MoneyCurrencyMismatchException(string left, string right)
            : base($"Cannot operate on money with different currencies: '{left}' vs '{right}'.") { }
    }
}
