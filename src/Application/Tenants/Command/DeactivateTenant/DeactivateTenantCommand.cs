using MediatR;


using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.DeactivateTenant
{
    public class DeactivateTenantCommand : IRequest<Result<DeactivateTenantResult>>
    {
        public Guid TenantId { get; }
        public DeactivateTenantCommand(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

}
