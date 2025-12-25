
using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Tenant;

namespace Application.Tenants.Command.SuspendTenant
{
    public class SuspendCommandHandler : ICommandHandler<SuspendTenantCommand, Result<SuspendTenantResult>>
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
            try
            {
                tenant.Suspend();
            }
            catch (InvalidCastException ex)
            {
                return Result<SuspendTenantResult>.Failure(ex.Message);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var suspendTenantResult = new SuspendTenantResult(tenant.Id, tenant.Status.ToString());
            return Result<SuspendTenantResult>.Success(suspendTenantResult);

        }
    }
}