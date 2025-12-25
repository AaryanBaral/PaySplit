

using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.ActivateTenant
{
    public class ActivateTenantCommand : ICommand<Result<ActivateTenantResult>>
    {
        public Guid TenantId { get; }
        public ActivateTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}
