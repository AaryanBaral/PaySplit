using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Tenant;
using Microsoft.Extensions.Logging;

namespace Application.Tenants.Command.CreateTenant
{
    public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, Result<CreateTenantResult>>
    {
        private readonly ITenantRepository _repostory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTenantHandler> _logger;
        public CreateTenantHandler(ITenantRepository repository, IUnitOfWork unitOfWork, ILogger<CreateTenantHandler> logger)
        {
            _repostory = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<CreateTenantResult>> HandleAsync(CreateTenantCommand command, CancellationToken cancellationToken = default)
        {
            Tenant tenant;

            try
            {
                _logger.LogInformation("Creating tenant {TenantName}", command.Name);
                tenant = Tenant.Create(command.Name);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Create tenant failed: {Error}", ex.Message);
                return Result<CreateTenantResult>.Failure(ex.Message);
            }

            await _repostory.AddAsync(tenant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createTenantResult = new CreateTenantResult(
                tenant.Id,
                tenant.Status.ToString()
            );
            _logger.LogInformation("Tenant created {TenantId} with status {Status}", tenant.Id, tenant.Status);
            return Result<CreateTenantResult>.Success(createTenantResult);
        }
    }
}
