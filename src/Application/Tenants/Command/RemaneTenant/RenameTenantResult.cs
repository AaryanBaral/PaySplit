

namespace PaySplit.Application.Tenants.Command.UpdateTenant
{
    public class RenameTenantResult
    {
        public Guid TenantId { get; }
        public string Status { get; }
        public string Name { get; }

        public RenameTenantResult(Guid tenantId, string name, string status)
        {
            TenantId = tenantId;
            Name = name;
            Status = status;
        }
    }
}
