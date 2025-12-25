using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantHandler : IQueryHandler<GetAllTenantQuery, Result<List<GetAllTenantDto>>>
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
                .Select(t => new GetAllTenantDto(
                    t.Id,
                    t.Name,
                    t.Status.ToString(),
                    t.CreatedAtUtc))
                .ToList();

            return Result<List<GetAllTenantDto>>.Success(result);
        }
    }
}
