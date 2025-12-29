using PaySplit.Domain.Tenants;
using Xunit;

namespace PaySplit.Domain.Tests.Tenants
{
    public class TenantTests
    {
        [Fact]
        public void Create_WithEmptyName_ShouldThrow()
        {
            var result = Tenant.Create(" ", "USD");

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Deactivate_ShouldSetTimestamps()
        {
            var tenant = Tenant.Create("Acme", "USD").Value!;

            tenant.Deactivate();

            Assert.Equal(TenantStatus.Inactive, tenant.Status);
            Assert.NotNull(tenant.DeactivatedAtUtc);
            Assert.Null(tenant.SuspendedAtUtc);
        }

        [Fact]
        public void Suspend_ShouldSetTimestamps()
        {
            var tenant = Tenant.Create("Acme", "USD").Value!;

            tenant.Suspend();

            Assert.Equal(TenantStatus.Suspended, tenant.Status);
            Assert.NotNull(tenant.SuspendedAtUtc);
            Assert.Null(tenant.DeactivatedAtUtc);
        }

        [Fact]
        public void Activate_ShouldClearTimestamps()
        {
            var tenant = Tenant.Create("Acme", "USD").Value!;
            tenant.Suspend();

            tenant.Activate();

            Assert.Equal(TenantStatus.Active, tenant.Status);
            Assert.Null(tenant.DeactivatedAtUtc);
            Assert.Null(tenant.SuspendedAtUtc);
        }
    }
}
