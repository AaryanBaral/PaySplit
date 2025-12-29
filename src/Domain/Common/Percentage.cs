

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

        public static Percentage Create(decimal value)
        {
            if (value <= 0 || value >= 100)
            {
                throw new Exceptions.PercentageOutOfRangeException(value);
            }
            return new Percentage(value);
        }

        public decimal AsFraction() => Value / 100m;
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;

        }
    }
}
