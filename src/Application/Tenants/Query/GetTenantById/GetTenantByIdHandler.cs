
using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdHandler : IQueryHandler<GetTenantByIdQuery, Result<GetTenantByIdDto>>
    {
        private readonly ITenantRepository _repository;
        public GetTenantByIdHandler(ITenantRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<GetTenantByIdDto>> HandleAsync(GetTenantByIdQuery query, CancellationToken cancellationToken = default)
        {
            if (query.Id == Guid.Empty)
            {
                return Result<GetTenantByIdDto>.Failure("Tenant id is required.");
            }

            var tenant = await _repository.GetByIdAsync(query.Id, cancellationToken);
            if (tenant is null)
            {
                return Result<GetTenantByIdDto>.Failure("Tenant not found");
            }
            var dto = new GetTenantByIdDto(
                tenant.Id,
                tenant.Name,
                tenant.Status.ToString(),
                tenant.CreatedAtUtc,
                tenant.DeactivatedAtUtc,
                tenant.SuspendedAtUtc);
            return Result<GetTenantByIdDto>.Success(dto);

        }
    }
}
