
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;
using MediatR;

namespace PaySplit.Application.Tenants.Command.SuspendTenant
{
    public class SuspendCommandHandler: IRequestHandler<SuspendTenantCommand, Result<SuspendTenantResult>>
    {
        private readonly ITenantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public SuspendCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SuspendTenantResult>> HandleAsync(SuspendTenantCommand command, CancellationToken cancellationToken = default)
        {
            var tenant = await _repository.GetByIdAsync(command.TenantId, cancellationToken);
            if (tenant is null)
            {
                return Result<SuspendTenantResult>.Failure("Tenant id is not valid");
            }
            if (tenant.Status == TenantStatus.Suspended)
            {
                return Result<SuspendTenantResult>.Failure("Tenant is already Suspended");
            }
            var suspendResult = tenant.Suspend();
            if (!suspendResult.IsSuccess)
            {
                return Result<SuspendTenantResult>.Failure(suspendResult.Error ?? "Tenant suspension failed.");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<SuspendTenantResult>.Success(tenant.ToSuspendTenantResult());

        }
    
        public Task<Result<SuspendTenantResult>> Handle(SuspendTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
