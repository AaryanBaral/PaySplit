using Domain.Tenants;
using Xunit;

namespace Domain.Tests.Tenants
{
    public class TenantTests
    {
        [Fact]
        public void Create_WithEmptyName_ShouldThrow()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Tenant.Create(" "));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void Deactivate_ShouldSetTimestamps()
        {
            var tenant = Tenant.Create("Acme");

            tenant.Deactivate();

            Assert.Equal(TenantStatus.Inactive, tenant.Status);
            Assert.NotNull(tenant.DeactivatedAtUtc);
            Assert.Null(tenant.SuspendedAtUTC);
        }

        [Fact]
        public void Suspend_ShouldSetTimestamps()
        {
            var tenant = Tenant.Create("Acme");

            tenant.Suspend();

            Assert.Equal(TenantStatus.Suspended, tenant.Status);
            Assert.NotNull(tenant.SuspendedAtUTC);
            Assert.Null(tenant.DeactivatedAtUtc);
        }

        [Fact]
        public void Activate_ShouldClearTimestamps()
        {
            var tenant = Tenant.Create("Acme");
            tenant.Suspend();

            tenant.Activate();

            Assert.Equal(TenantStatus.Active, tenant.Status);
            Assert.Null(tenant.DeactivatedAtUtc);
            Assert.Null(tenant.SuspendedAtUTC);
        }
    }
}
