
using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.ActivateTenant
{
    public class ActivateCommandHandler : ICommandHandler<ActivateTenantCommand, Result<ActivateTenantResult>>
    {
        private readonly ITenantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ActivateCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ActivateTenantResult>> HandleAsync(ActivateTenantCommand command, CancellationToken cancellationToken = default)
        {
            var tenant = await _repository.GetByIdAsync(command.TenantId, cancellationToken);
            if (tenant is null)
            {
                return Result<ActivateTenantResult>.Failure("Tenant id is not valid");
            }
            if (tenant.Status == TenantStatus.Active)
            {
                return Result<ActivateTenantResult>.Failure("Tenant is already Activated");
            }
            try
            {
                tenant.Activate();
            }
            catch (InvalidCastException ex)
            {
                return Result<ActivateTenantResult>.Failure(ex.Message);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var activateTenantResult = new ActivateTenantResult(tenant.Id, tenant.Status.ToString());
            return Result<ActivateTenantResult>.Success(activateTenantResult);

        }
    }
}
