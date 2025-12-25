namespace Domain.Common
{
    public class Money : ValueObject
    {
        public string Currency { get; protected set; }= default!;
        public decimal Amount { get; protected set; }

        // Required by Ef Core
   
        private Money() { }

       

        private Money(string currency, decimal amount)
        {
            this.Currency = currency;
            this.Amount = amount;
        }

        public static Money Create(string currency, decimal amount)
        {
            if (String.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }
            if (amount < 0)
            {
                throw new ArgumentException("Amount Cannot be negative", nameof(currency));
            }

            return new Money(currency, amount);
        }

        public static Money CreteZreo(string currency) => Create(currency, 0m);

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
            return new Money(another.Currency, another.Amount + Amount);
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