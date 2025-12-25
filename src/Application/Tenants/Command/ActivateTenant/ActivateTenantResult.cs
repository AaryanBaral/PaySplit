

using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.ActivateTenant
{
    public class ActivateTenantResult
    {
        public Guid TenantId { get; }
        public string Status { get; }

        public ActivateTenantResult(Guid tenantId, string status)
        {
            TenantId = tenantId;
            Status = status;
        }
    }
}
