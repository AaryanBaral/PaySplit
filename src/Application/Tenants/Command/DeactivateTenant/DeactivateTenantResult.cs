

using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.DeactivateTenant
{
    public class DeactivateTenantResult
    {
        public Guid TenantId { get; }
        public string Status { get; }

        public DeactivateTenantResult(Guid tenantId, string status)
        {
            TenantId = tenantId;
            Status = status;
        }
    }
}
