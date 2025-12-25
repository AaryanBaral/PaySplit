using PaySplit.Application.Common.Filter;
using PaySplit.Domain.Tenants;

namespace PaySplit.Application.Interfaces.Repository;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Tenant tenant, CancellationToken ct = default);
    Task<List<Tenant>> GetAllAsync(PaginationFilter filter, CancellationToken ct = default);
}
