using PaySplit.Domain.Payouts;
using Xunit;

namespace PaySplit.Domain.Tests.Payouts
{
    public class PayoutTests
    {
        [Fact]
        public void Request_WithEmptyTenantId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payout.Request(Guid.Empty, Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Request_WithEmptyMerchantId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payout.Request(Guid.NewGuid(), Guid.Empty, 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Request_WithEmptyRequestedBy_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.Empty, DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Request_WithDefaultRequestedAt_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), default));
        }

        [Fact]
        public void Approve_WithEmptyUserId_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow);

            Assert.Throws<ArgumentException>(() => payout.Approve(Guid.Empty, DateTimeOffset.UtcNow));
        }

        [Fact]
        public void MarkCompleted_WithEmptyUserId_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow);
            payout.Approve(Guid.NewGuid(), DateTimeOffset.UtcNow);

            Assert.Throws<ArgumentException>(() => payout.MarkCompleted(Guid.Empty, DateTimeOffset.UtcNow));
        }

        [Fact]
        public void Reject_WithDefaultTime_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow);

            Assert.Throws<ArgumentException>(() => payout.Reject(Guid.NewGuid(), default));
        }
    }
}
