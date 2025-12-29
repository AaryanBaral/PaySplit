
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;
using MediatR;

namespace PaySplit.Application.Tenants.Command.DeactivateTenant
{
    public class DeactivateCommandHandler: IRequestHandler<DeactivateTenantCommand, Result<DeactivateTenantResult>>
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
            var deactivateResult = tenant.Deactivate();
            if (!deactivateResult.IsSuccess)
            {
                return Result<DeactivateTenantResult>.Failure(deactivateResult.Error ?? "Tenant deactivation failed.");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<DeactivateTenantResult>.Success(tenant.ToDeactivateTenantResult());

        }
    
        public Task<Result<DeactivateTenantResult>> Handle(DeactivateTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
