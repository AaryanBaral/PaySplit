using PaySplit.Application.Common.Filter;
using PaySplit.Domain.Merchants;

namespace PaySplit.Application.Interfaces.Repository;

public interface IMerchantRepository
{
    Task<Merchant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Merchant merchant, CancellationToken ct = default);
    Task<List<Merchant>> GetAllAsync(PaginationFilter filter, CancellationToken ct = default);
}
