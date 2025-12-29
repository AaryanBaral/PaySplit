using PaySplit.Domain.Payouts;
using Xunit;

namespace PaySplit.Domain.Tests.Payouts
{
    public class PayoutTests
    {
        [Fact]
        public void Request_WithEmptyTenantId_ShouldThrow()
        {
            var result = Payout.Request(Guid.Empty, Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Request_WithEmptyMerchantId_ShouldThrow()
        {
            var result = Payout.Request(Guid.NewGuid(), Guid.Empty, 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Request_WithEmptyRequestedBy_ShouldThrow()
        {
            var result = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.Empty, DateTimeOffset.UtcNow);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Request_WithDefaultRequestedAt_ShouldThrow()
        {
            var result = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), default);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Approve_WithEmptyUserId_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow).Value!;

            var result = payout.Approve(Guid.Empty, DateTimeOffset.UtcNow);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void MarkCompleted_WithEmptyUserId_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow).Value!;
            payout.Approve(Guid.NewGuid(), DateTimeOffset.UtcNow);

            var result = payout.MarkCompleted(Guid.Empty, DateTimeOffset.UtcNow);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Reject_WithDefaultTime_ShouldThrow()
        {
            var payout = Payout.Request(Guid.NewGuid(), Guid.NewGuid(), 10m, "USD", Guid.NewGuid(), DateTimeOffset.UtcNow).Value!;

            var result = payout.Reject(Guid.NewGuid(), default);
            Assert.False(result.IsSuccess);
        }
    }
}
