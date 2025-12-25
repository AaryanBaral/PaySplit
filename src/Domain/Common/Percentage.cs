

namespace Domain.Common
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
                throw new ArgumentException("Percentage must be between 0 and 100 (exclusive).");
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