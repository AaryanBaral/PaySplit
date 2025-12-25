using PaySplit.Domain.Common;

using Xunit;

namespace PaySplit.Domain.Tests.Common
{
    public class PercentageTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldReturnPercentage()
        {
            var percentage = Percentage.Create(12.5m);

            Assert.Equal(12.5m, percentage.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-1)]
        public void Create_WithInvalidValue_ShouldThrow(decimal value)
        {
            var exception = Assert.Throws<ArgumentException>(() => Percentage.Create(value));

            Assert.Equal("Percentage must be between 0 and 100 (exclusive).", exception.Message);
        }

        [Fact]
        public void AsFraction_ShouldReturnDecimalFraction()
        {
            var percentage = Percentage.Create(12.5m);

            Assert.Equal(0.125m, percentage.AsFraction());
        }
    }
}
