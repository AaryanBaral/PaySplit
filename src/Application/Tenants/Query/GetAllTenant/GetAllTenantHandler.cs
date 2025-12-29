using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantHandler: IRequestHandler<GetAllTenantQuery, Result<List<GetAllTenantDto>>>
    {
        private readonly ITenantRepository _repository;
        public GetAllTenantHandler(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetAllTenantDto>>> HandleAsync(GetAllTenantQuery query, CancellationToken cancellationToken = default)
        {
            var filter = query.PaginationFilter;

            var tenants = await _repository.GetAllAsync(filter, cancellationToken);

            var result = tenants
                .Select(t => t.ToGetAllTenantDto())
                .ToList();

            return Result<List<GetAllTenantDto>>.Success(result);
        }
    
        public Task<Result<List<GetAllTenantDto>>> Handle(GetAllTenantQuery request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
