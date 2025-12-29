

using PaySplit.Domain.Common.Results;

namespace PaySplit.Domain.Common
{
    public class Percentage : ValueObject
    {
        public decimal Value { get; private set; }

        private Percentage() { }

        private Percentage(decimal value)
        {
            Value = value;
        }

        public static Result<Percentage> Create(decimal value)
        {
            if (value <= 0 || value >= 100)
            {
                return Result<Percentage>.Failure("Percentage must be between 0 and 100 (exclusive).");
            }
            return Result<Percentage>.Success(new Percentage(value));
        }

        public decimal AsFraction() => Value / 100m;
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;

        }
    }
}
