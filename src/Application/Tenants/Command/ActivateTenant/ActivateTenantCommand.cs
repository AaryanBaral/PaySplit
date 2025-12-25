

using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Command.ActivateTenant
{
    public class ActivateTenantCommand:ICommand<Result<ActivateTenantResult>>
    {
        public Guid TenantId {get;}
        public ActivateTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}