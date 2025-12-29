
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdHandler: IRequestHandler<GetTenantByIdQuery, Result<GetTenantByIdDto>>
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
            return Result<GetTenantByIdDto>.Success(tenant.ToGetTenantByIdDto());

        }
    
        public Task<Result<GetTenantByIdDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
