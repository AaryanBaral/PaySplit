

using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.SuspendTenant
{
    public class SuspendTenantResult
    {
        public Guid TenantId { get; }
        public string Status { get; }

        public SuspendTenantResult(Guid tenantId, string status)
        {
            TenantId = tenantId;
            Status = status;
        }
    }
}
