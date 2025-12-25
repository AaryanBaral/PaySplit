using Domain.Common;
using Xunit;

namespace Domain.Tests.Common
{
    public class MoneyTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnMoney()
        {
            var money = Money.Create("USD", 10.50m);

            Assert.Equal("USD", money.Currency);
            Assert.Equal(10.50m, money.Amount);
        }

        [Fact]
        public void Create_WithEmptyCurrency_ShouldThrow()
        {
            var exception = Assert.Throws<ArgumentException>(() => Money.Create("", 5m));

            Assert.Equal("Currency is required. (Parameter 'currency')", exception.Message);
        }

        [Fact]
        public void Create_WithNegativeAmount_ShouldThrow()
        {
            var exception = Assert.Throws<ArgumentException>(() => Money.Create("USD", -1m));

            Assert.Equal("Amount Cannot be negative (Parameter 'currency')", exception.Message);
        }

        [Fact]
        public void CreteZreo_ShouldReturnZeroAmount()
        {
            var money = Money.CreateZreo("USD");

            Assert.Equal("USD", money.Currency);
            Assert.Equal(0m, money.Amount);
        }

        [Fact]
        public void Add_WithSameCurrency_ShouldSum()
        {
            var first = Money.Create("USD", 10m);
            var second = Money.Create("USD", 5m);

            var result = first.Add(second);

            Assert.Equal("USD", result.Currency);
            Assert.Equal(15m, result.Amount);
        }

        [Fact]
        public void Add_WithDifferentCurrency_ShouldThrow()
        {
            var first = Money.Create("USD", 10m);
            var second = Money.Create("EUR", 5m);

            var exception = Assert.Throws<InvalidOperationException>(() => first.Add(second));

            Assert.Equal("Cannot operate on money with different currencies.", exception.Message);
        }

        [Fact]
        public void Subtract_WithInsufficientAmount_ShouldThrow()
        {
            var first = Money.Create("USD", 5m);
            var second = Money.Create("USD", 10m);

            var exception = Assert.Throws<InvalidOperationException>(() => first.Subtract(second));

            Assert.Equal("Resulting money cannot be negative.", exception.Message);
        }

        [Fact]
        public void Subtract_WithSameCurrency_ShouldReturnDifference()
        {
            var first = Money.Create("USD", 10m);
            var second = Money.Create("USD", 3m);

            var result = first.Subtract(second);

            Assert.Equal("USD", result.Currency);
            Assert.Equal(7m, result.Amount);
        }

        [Fact]
        public void Equals_WithSameValues_ShouldBeTrue()
        {
            var first = Money.Create("USD", 10m);
            var second = Money.Create("USD", 10m);

            Assert.True(first.Equals(second));
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }
    }
}
