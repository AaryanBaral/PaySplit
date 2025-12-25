using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.CreateTenant
{
    public class CreateTenantResult
    {
        public Guid TenantId { get; }
        public string Status { get; }

        public CreateTenantResult(Guid tenantId, string status)
        {
            TenantId = tenantId;
            Status = status;
        }
    }
}
