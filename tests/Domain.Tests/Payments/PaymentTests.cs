using PaySplit.Domain.Common;
using PaySplit.Domain.Payments;
using Xunit;

namespace PaySplit.Domain.Tests.Payments
{
    public class PaymentTests
    {
        [Fact]
        public void CreatePending_WithEmptyTenantId_ShouldThrow()
        {
            var result = Payment.CreatePending(Guid.Empty, Guid.NewGuid(), 10m, "USD");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreatePending_WithEmptyMerchantId_ShouldThrow()
        {
            var result = Payment.CreatePending(Guid.NewGuid(), Guid.Empty, 10m, "USD");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreatePending_WithInvalidAmount_ShouldThrow()
        {
            var result = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 0m, "USD");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreatePending_WithMissingExternalId_ShouldSucceed()
        {
            var result = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD");
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CreateIncoming_WithMissingExternalId_ShouldFail()
        {
            var result = Payment.CreateIncoming(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", " ");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void MarkSucceeded_WithDefaultTime_ShouldThrow()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD").Value!;

            var result = payment.MarkSucceeded(default);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CalculateRevenueSplit_WhenPending_ShouldThrow()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD").Value!;
            var share = Percentage.Create(10m).Value!;

            var result = payment.CalculateRevenueSplit(share);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CalculateRevenueSplit_WhenSucceeded_ShouldReturnSplit()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 100m, "USD").Value!;
            payment.MarkSucceeded(DateTimeOffset.UtcNow);
            var share = Percentage.Create(25m).Value!;

            var result = payment.CalculateRevenueSplit(share);
            var (merchantAmount, tenantAmount) = result.Value!;

            Assert.Equal(25m, merchantAmount.Amount);
            Assert.Equal(75m, tenantAmount.Amount);
            Assert.Equal("USD", merchantAmount.Currency);
            Assert.Equal("USD", tenantAmount.Currency);
        }
    }
}
