

using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Command.SuspendTenant
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