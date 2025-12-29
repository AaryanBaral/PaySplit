using PaySplit.Domain.Common.Results;

namespace PaySplit.Domain.Common
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

        public static Result<Money> Create(string currency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                return Result<Money>.Failure("Currency is required.");
            }
            if (amount < 0)
            {
                return Result<Money>.Failure($"Amount cannot be negative. Received: {amount}.");
            }

            return Result<Money>.Success(new Money(currency.Trim().ToUpperInvariant(), amount));
        }

        public static Result<Money> CreateZero(string currency) => Create(currency, 0m);

        public Money Subtract(Money another)
        {
            EnsureSameCurrency(another);
            if (Amount < another.Amount)
            {
                throw new Exceptions.MoneyResultNegativeException();
            }
            var resultAmount = Amount - another.Amount;
            return new Money(Currency, resultAmount);
        }
        public Money Multiply(decimal factor)
        {
            if (factor < 0)
            {
                throw new Exceptions.MoneyAmountNegativeException(factor);
            }

            var result = Amount * factor;
            return new Money(Currency, result);
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
                throw new Exceptions.MoneyCurrencyMismatchException(Currency, another.Currency);
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
