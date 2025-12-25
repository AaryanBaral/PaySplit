namespace Domain.Common
{
    public class Money : ValueObject
    {
        public string Currency { get; private set; } = default!;
        public decimal Amount { get; private set; }

        // Required by Ef Core

        private Money() { }



        private Money(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        public static Money Create(string currency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }
            if (amount < 0)
            {
                throw new ArgumentException("Amount Cannot be negative", nameof(amount));
            }

            return new Money(currency.Trim().ToUpperInvariant(), amount);
        }

        public static Money CreateZero(string currency) => Create(currency, 0m);

        public Money Subtract(Money another)
        {
            EnsureSameCurrency(another);
            if (Amount < another.Amount)
            {
                throw new InvalidOperationException("Resulting money cannot be negative.");
            }
            var resultAmount = Amount - another.Amount;
            return new Money(Currency, resultAmount);
        }
        public Money Add(Money another)
        {
            EnsureSameCurrency(another);
            return new Money(Currency, another.Amount + Amount);
        }
        private void EnsureSameCurrency(Money another)
        {
            if (!Currency.Equals(another.Currency, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Cannot operate on money with different currencies.");
            }
        }
        public override string ToString() => $"{Amount} {Currency}";

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;

        }

    }
}
