

using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.SuspendTenant
{
    public class SuspendTenantCommand : ICommand<Result<SuspendTenantResult>>
    {
        public Guid TenantId { get; }
        public SuspendTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}
