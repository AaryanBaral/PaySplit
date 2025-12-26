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
            Assert.Throws<ArgumentException>(() =>
                Payment.CreatePending(Guid.Empty, Guid.NewGuid(), 10m, "USD", "ext-1"));
        }

        [Fact]
        public void CreatePending_WithEmptyMerchantId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payment.CreatePending(Guid.NewGuid(), Guid.Empty, 10m, "USD", "ext-1"));
        }

        [Fact]
        public void CreatePending_WithInvalidAmount_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 0m, "USD", "ext-1"));
        }

        [Fact]
        public void CreatePending_WithMissingExternalId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", " "));
        }

        [Fact]
        public void MarkSucceeded_WithDefaultTime_ShouldThrow()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", "ext-1");

            Assert.Throws<ArgumentException>(() => payment.MarkSucceeded(default));
        }

        [Fact]
        public void CalculateRevenueSplit_WhenPending_ShouldThrow()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", "ext-1");
            var share = Percentage.Create(10m);

            Assert.Throws<InvalidOperationException>(() => payment.CalculateRevenueSplit(share));
        }

        [Fact]
        public void CalculateRevenueSplit_WhenSucceeded_ShouldReturnSplit()
        {
            var payment = Payment.CreatePending(Guid.NewGuid(), Guid.NewGuid(), 100m, "USD", "ext-1");
            payment.MarkSucceeded(DateTimeOffset.UtcNow);
            var share = Percentage.Create(25m);

            var (merchantAmount, tenantAmount) = payment.CalculateRevenueSplit(share);

            Assert.Equal(25m, merchantAmount.Amount);
            Assert.Equal(75m, tenantAmount.Amount);
            Assert.Equal("USD", merchantAmount.Currency);
            Assert.Equal("USD", tenantAmount.Currency);
        }
    }
}
