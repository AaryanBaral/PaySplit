using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Tenant;

namespace Application.Tenants.Command.CreateTenant
{
    public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, Result<CreateTenantResult>>
    {
        private readonly ITenantRepository _repostory;
        private readonly IUnitOfWork _unitOfWork;
        public CreateTenantHandler(ITenantRepository repository, IUnitOfWork unitOfWork)
        {
            _repostory = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateTenantResult>> HandleAsync(CreateTenantCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<CreateTenantResult>.Failure("Tenant name is required.");
            }
            Tenant tenant;

            try
            {
                tenant = Tenant.Create(
                    command.Name,
                    DateTimeOffset.UtcNow
                );
            }
            catch (ArgumentException ex)
            {
                return Result<CreateTenantResult>.Failure(ex.Message);
            }

            await _repostory.AddAsync(tenant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createTenantResult = new CreateTenantResult(
                tenant.Id,
                tenant.Status.ToString()
            );
            return Result<CreateTenantResult>.Success(createTenantResult);
        }
    }
}