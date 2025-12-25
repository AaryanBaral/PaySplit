using Application.Interfaces.Repository;
using Domain.Merchant;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly AppDbContext _dbContext;

        public MerchantRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Merchants
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task AddAsync(Merchant merchant, CancellationToken cancellationToken = default)
        {
            await _dbContext.Merchants.AddAsync(merchant, cancellationToken);
        }
    }
}
