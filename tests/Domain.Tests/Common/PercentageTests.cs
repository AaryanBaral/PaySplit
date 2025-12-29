using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Exceptions;
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
            Assert.Throws<PercentageOutOfRangeException>(() => Percentage.Create(value));
        }

        [Fact]
        public void AsFraction_ShouldReturnDecimalFraction()
        {
            var percentage = Percentage.Create(12.5m);

            Assert.Equal(0.125m, percentage.AsFraction());
        }
    }
}
