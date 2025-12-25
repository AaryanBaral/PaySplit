using Domain.Merchant;

namespace Application.Interfaces.Repository;

public interface IMerchantRepository
{
    Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Merchant merchant, CancellationToken cancellationToken);
}
