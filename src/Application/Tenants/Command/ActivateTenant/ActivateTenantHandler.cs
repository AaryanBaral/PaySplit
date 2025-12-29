
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Common.Exceptions;
using PaySplit.Domain.Tenants;
using MediatR;

namespace PaySplit.Application.Tenants.Command.ActivateTenant
{
    public class ActivateCommandHandler: IRequestHandler<ActivateTenantCommand, Result<ActivateTenantResult>>
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
            catch (DomainException ex)
            {
                return Result<ActivateTenantResult>.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<ActivateTenantResult>.Failure(ex.Message);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ActivateTenantResult>.Success(tenant.ToActivateTenantResult());

        }
    
        public Task<Result<ActivateTenantResult>> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
