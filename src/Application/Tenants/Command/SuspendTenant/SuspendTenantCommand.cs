using MediatR;


using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.SuspendTenant
{
    public class SuspendTenantCommand : IRequest<Result<SuspendTenantResult>>
    {
        public Guid TenantId { get; }
        public SuspendTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}
