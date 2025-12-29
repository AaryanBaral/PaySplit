using PaySplit.Domain.Common.Exceptions;
using PaySplit.Domain.Merchants;

namespace PaySplit.Domain.Tests.Merchants
{
    public class MerchantTests
    {
        [Fact]
        public void Create_WithEmptyTenantId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Merchant.Create(Guid.Empty, "Test", "test@example.com", 10m));
        }

        [Fact]
        public void Create_WithInvalidRevenueShare_ShouldThrow()
        {
            Assert.Throws<PercentageOutOfRangeException>(() =>
                Merchant.Create(Guid.NewGuid(), "Test", "test@example.com", 0m));
        }

        [Fact]
        public void Deactivate_ShouldSetTimestamps()
        {
            var merchant = Merchant.Create(Guid.NewGuid(), "Test", "test@example.com", 10m);

            merchant.Deactivate();

            Assert.Equal(MerchantStatus.Inactive, merchant.Status);
            Assert.NotNull(merchant.DeactivatedAtUtc);
            Assert.Null(merchant.SuspendedAtUtc);
        }

        [Fact]
        public void Suspend_ShouldSetTimestamps()
        {
            var merchant = Merchant.Create(Guid.NewGuid(), "Test", "test@example.com", 10m);

            merchant.Suspend();

            Assert.Equal(MerchantStatus.Suspended, merchant.Status);
            Assert.NotNull(merchant.SuspendedAtUtc);
            Assert.Null(merchant.DeactivatedAtUtc);
        }

        [Fact]
        public void Activate_ShouldClearTimestamps()
        {
            var merchant = Merchant.Create(Guid.NewGuid(), "Test", "test@example.com", 10m);
            merchant.Suspend();

            merchant.Activate();

            Assert.Equal(MerchantStatus.Active, merchant.Status);
            Assert.Null(merchant.DeactivatedAtUtc);
            Assert.Null(merchant.SuspendedAtUtc);
        }
    }
}
