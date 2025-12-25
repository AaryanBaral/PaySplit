using Domain.Tenant;

namespace Application.Tenants.Command.CreateTenant
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