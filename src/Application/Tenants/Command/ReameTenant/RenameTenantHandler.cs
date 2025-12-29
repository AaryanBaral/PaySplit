
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Common.Exceptions;
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
            try
            {
                tenant.Rename(command.Name);
            }
            catch (DomainException ex)
            {
                return Result<RenameTenantResult>.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<RenameTenantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<RenameTenantResult>.Success(tenant.ToRenameTenantResult());

        }
    
        public Task<Result<RenameTenantResult>> Handle(RenameTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
