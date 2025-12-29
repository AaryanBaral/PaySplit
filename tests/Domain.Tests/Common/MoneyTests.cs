using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Exceptions;
using Xunit;

namespace PaySplit.Domain.Tests.Common
{
    public class MoneyTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnMoney()
        {
            var result = Money.Create("USD", 10.50m);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("USD", result.Value!.Currency);
            Assert.Equal(10.50m, result.Value.Amount);
        }

        [Fact]
        public void Create_WithEmptyCurrency_ShouldThrow()
        {
            var result = Money.Create("", 5m);
            Assert.False(result.IsSuccess);
            Assert.Equal("Currency is required.", result.Error);
        }

        [Fact]
        public void Create_WithNegativeAmount_ShouldThrow()
        {
            var result = Money.Create("USD", -1m);
            Assert.False(result.IsSuccess);
            Assert.Equal("Amount cannot be negative. Received: -1.", result.Error);
        }

        [Fact]
        public void CreateZero_ShouldReturnZeroAmount()
        {
            var result = Money.CreateZero("USD");

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("USD", result.Value!.Currency);
            Assert.Equal(0m, result.Value.Amount);
        }

        [Fact]
        public void Add_WithSameCurrency_ShouldSum()
        {
            var first = Money.Create("USD", 10m).Value!;
            var second = Money.Create("USD", 5m).Value!;

            var result = first.Add(second);

            Assert.Equal("USD", result.Currency);
            Assert.Equal(15m, result.Amount);
        }

        [Fact]
        public void Create_ShouldNormalizeCurrency()
        {
            var result = Money.Create(" usd ", 1m);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("USD", result.Value!.Currency);
        }

        [Fact]
        public void Add_WithDifferentCurrency_ShouldThrow()
        {
            var first = Money.Create("USD", 10m).Value!;
            var second = Money.Create("EUR", 5m).Value!;

            Assert.Throws<MoneyCurrencyMismatchException>(() => first.Add(second));
        }

        [Fact]
        public void Subtract_WithInsufficientAmount_ShouldThrow()
        {
            var first = Money.Create("USD", 5m).Value!;
            var second = Money.Create("USD", 10m).Value!;

            Assert.Throws<MoneyResultNegativeException>(() => first.Subtract(second));
        }

        [Fact]
        public void Subtract_WithSameCurrency_ShouldReturnDifference()
        {
            var first = Money.Create("USD", 10m).Value!;
            var second = Money.Create("USD", 3m).Value!;

            var result = first.Subtract(second);

            Assert.Equal("USD", result.Currency);
            Assert.Equal(7m, result.Amount);
        }

        [Fact]
        public void Equals_WithSameValues_ShouldBeTrue()
        {
            var first = Money.Create("USD", 10m).Value!;
            var second = Money.Create("USD", 10m).Value!;

            Assert.True(first.Equals(second));
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }
    }
}
