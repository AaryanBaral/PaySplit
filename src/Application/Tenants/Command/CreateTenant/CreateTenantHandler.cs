using Microsoft.Extensions.Logging;
using MediatR;

using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Tenants.Command.CreateTenant
{
    public class CreateTenantHandler: IRequestHandler<CreateTenantCommand, Result<CreateTenantResult>>
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
            _logger.LogInformation("Creating tenant {TenantName} with currency {DefaultCurrency}", command.Name, command.DefaultCurrency);
            var tenantResult = Tenant.Create(command.Name, command.DefaultCurrency ?? "USD");
            if (!tenantResult.IsSuccess || tenantResult.Value is null)
            {
                var error = tenantResult.Error ?? "Tenant is invalid.";
                _logger.LogWarning("Create tenant failed: {Error}", error);
                return Result<CreateTenantResult>.Failure(error);
            }

            var tenant = tenantResult.Value;
            await _repository.AddAsync(tenant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = tenant.ToCreateTenantResult();
            _logger.LogInformation("Tenant created {TenantId} with status {Status}", tenant.Id, tenant.Status);
            return Result<CreateTenantResult>.Success(result);
        }
    
        public Task<Result<CreateTenantResult>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
