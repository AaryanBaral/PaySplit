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
            Assert.Throws<ArgumentException>(() =>
                LedgerEntry.CreateMerchantCredit(
                    Guid.Empty,
                    Guid.NewGuid(),
                    Money.Create("USD", 10m),
                    LedgerEntrySourceType.Payment,
                    Guid.NewGuid(),
                    "desc",
                    DateTimeOffset.UtcNow));
        }

        [Fact]
        public void CreateMerchantCredit_WithEmptySourceId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                LedgerEntry.CreateMerchantCredit(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Money.Create("USD", 10m),
                    LedgerEntrySourceType.Payment,
                    Guid.Empty,
                    "desc",
                    DateTimeOffset.UtcNow));
        }

        [Fact]
        public void CreateTenantCredit_ShouldSucceed()
        {
            var entry = LedgerEntry.CreateTenantCredit(
                Guid.NewGuid(),
                Money.Create("USD", 10m),
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                "desc",
                DateTimeOffset.UtcNow);

            Assert.Equal(LedgerEntryKind.TenantCredit, entry.Kind);
            Assert.Null(entry.MerchantId);
        }

        [Fact]
        public void CreateMerchantCredit_WithEmptyDescription_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                LedgerEntry.CreateMerchantCredit(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Money.Create("USD", 10m),
                    LedgerEntrySourceType.Payment,
                    Guid.NewGuid(),
                    " ",
                    DateTimeOffset.UtcNow));
        }

        [Fact]
        public void CreateMerchantCredit_ShouldSucceed()
        {
            var entry = LedgerEntry.CreateMerchantCredit(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Money.Create("USD", 10m),
                LedgerEntrySourceType.Payment,
                Guid.NewGuid(),
                "desc",
                DateTimeOffset.UtcNow);

            Assert.Equal(LedgerEntryKind.MerchantCredit, entry.Kind);
            Assert.NotNull(entry.MerchantId);
        }
    }
}
