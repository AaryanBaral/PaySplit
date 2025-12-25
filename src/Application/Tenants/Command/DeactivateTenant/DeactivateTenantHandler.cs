
using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.DeactivateTenant
{
    public class DeactivateCommandHandler : ICommandHandler<DeactivateTenantCommand, Result<DeactivateTenantResult>>
    {
        private readonly ITenantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeactivateCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeactivateTenantResult>> HandleAsync(DeactivateTenantCommand command, CancellationToken cancellationToken = default)
        {
            var tenant = await _repository.GetByIdAsync(command.TenantId, cancellationToken);
            if (tenant is null)
            {
                return Result<DeactivateTenantResult>.Failure("Tenant id is not valid");
            }
            if (tenant.Status == TenantStatus.Inactive)
            {
                return Result<DeactivateTenantResult>.Failure("Tenant is already Deactivated");
            }
            try
            {
                tenant.Deactivate();
            }
            catch (InvalidCastException ex)
            {
                return Result<DeactivateTenantResult>.Failure(ex.Message);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var deactivateTenantResult = new DeactivateTenantResult(tenant.Id, tenant.Status.ToString());
            return Result<DeactivateTenantResult>.Success(deactivateTenantResult);

        }
    }
}
