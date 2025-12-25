
using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Tenants.Command.UpdateTenant
{
    public class UpdateCommandHandler : ICommandHandler<RenameTenantCommand, Result<RenameTenantResult>>
    {
        private readonly ITenantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<RenameTenantResult>> HandleAsync(RenameTenantCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<RenameTenantResult>.Failure("Tenant name is required");
            }
            var tenant = await _repository.GetByIdAsync(command.Id);
            if (tenant is null)
            {
                return Result<RenameTenantResult>.Failure("Tenant not found");
            }
            try
            {
                tenant.Rename(command.Name);
            }
            catch (ArgumentException ex)
            {
                return Result<RenameTenantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync();
            var renameTenantResult = new RenameTenantResult(tenant.Id, tenant.Name, tenant.Status.ToString());
            return Result<RenameTenantResult>.Success(renameTenantResult);

        }
    }
}
