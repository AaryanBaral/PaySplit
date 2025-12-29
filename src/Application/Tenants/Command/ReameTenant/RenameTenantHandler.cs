
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Tenants.Command.UpdateTenant
{
    public class UpdateCommandHandler: IRequestHandler<RenameTenantCommand, Result<RenameTenantResult>>
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
            var renameResult = tenant.Rename(command.Name);
            if (!renameResult.IsSuccess)
            {
                return Result<RenameTenantResult>.Failure(renameResult.Error ?? "Tenant name is invalid.");
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<RenameTenantResult>.Success(tenant.ToRenameTenantResult());

        }
    
        public Task<Result<RenameTenantResult>> Handle(RenameTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
