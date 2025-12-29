using PaySplit.Domain.Common;
using PaySplit.Domain.Ledgers;
using Xunit;

namespace PaySplit.Domain.Tests.Ledgers
{
    public class LedgerEntryTests
    {
        [Fact]
        public void CreateMerchantCredit_WithEmptyTenantId_ShouldThrow()
        {
            var amount = Money.Create("USD", 10m).Value!;
            var result = LedgerEntry.CreateMerchantCredit(
                Guid.Empty,
                Guid.NewGuid(),
                amount,
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                "desc",
                DateTimeOffset.UtcNow);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreateMerchantCredit_WithEmptySourceId_ShouldThrow()
        {
            var amount = Money.Create("USD", 10m).Value!;
            var result = LedgerEntry.CreateMerchantCredit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                amount,
                LedgerEntrySourceType.Payment,
                Guid.Empty,
                "desc",
                DateTimeOffset.UtcNow);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreateTenantCredit_ShouldSucceed()
        {
            var amount = Money.Create("USD", 10m).Value!;
            var result = LedgerEntry.CreateTenantCredit(
                Guid.NewGuid(),
                amount,
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                "desc",
                DateTimeOffset.UtcNow);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(LedgerEntryKind.TenantCredit, result.Value!.Kind);
            Assert.Null(result.Value.MerchantId);
        }

        [Fact]
        public void CreateMerchantCredit_WithEmptyDescription_ShouldThrow()
        {
            var amount = Money.Create("USD", 10m).Value!;
            var result = LedgerEntry.CreateMerchantCredit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                amount,
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                " ",
                DateTimeOffset.UtcNow);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void CreateMerchantCredit_ShouldSucceed()
        {
            var amount = Money.Create("USD", 10m).Value!;
            var result = LedgerEntry.CreateMerchantCredit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                amount,
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                "desc",
                DateTimeOffset.UtcNow);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(LedgerEntryKind.MerchantCredit, result.Value!.Kind);
            Assert.NotNull(result.Value.MerchantId);
        }
    }
}
