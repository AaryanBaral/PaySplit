

using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Command.DeactivateTenant
{
    public class DeactivateTenantCommand : ICommand<Result<DeactivateTenantResult>>
    {
        public Guid TenantId { get; }
        public DeactivateTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}