using PaySplit.Domain.Common;
using Xunit;

namespace PaySplit.Domain.Tests.Common
{
    public class PercentageTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldReturnPercentage()
        {
            var result = Percentage.Create(12.5m);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(12.5m, result.Value!.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-1)]
        public void Create_WithInvalidValue_ShouldThrow(decimal value)
        {
            var result = Percentage.Create(value);
            Assert.False(result.IsSuccess);
            Assert.Equal("Percentage must be between 0 and 100 (exclusive).", result.Error);
        }

        [Fact]
        public void AsFraction_ShouldReturnDecimalFraction()
        {
            var percentage = Percentage.Create(12.5m).Value!;

            Assert.Equal(0.125m, percentage.AsFraction());
        }
    }
}
