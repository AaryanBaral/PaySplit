using Microsoft.Extensions.Logging;

using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.CreateTenant
{
    public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, Result<CreateTenantResult>>
    {
        private readonly ITenantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTenantHandler> _logger;
        public CreateTenantHandler(ITenantRepository repository, IUnitOfWork unitOfWork, ILogger<CreateTenantHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<CreateTenantResult>> HandleAsync(CreateTenantCommand command, CancellationToken cancellationToken = default)
        {
            Tenant tenant;

            try
            {
                _logger.LogInformation("Creating tenant {TenantName} with currency {DefaultCurrency}", command.Name, command.DefaultCurrency);
                tenant = Tenant.Create(command.Name, command.DefaultCurrency ?? "USD");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Create tenant failed: {Error}", ex.Message);
                return Result<CreateTenantResult>.Failure(ex.Message);
            }

            await _repository.AddAsync(tenant, cancellationToken);
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
