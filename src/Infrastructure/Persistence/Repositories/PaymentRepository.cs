using Microsoft.EntityFrameworkCore;

using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Payments;

namespace PaySplit.Infrastructure.Persistence.Repositories
{
    public sealed class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _dbContext;

        public PaymentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            await _dbContext.Payments.AddAsync(payment, cancellationToken);
        }
        public async Task<Payment?> GetByExternalIdAsync(Guid tenantId, string externalPaymentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.ExternalPaymentId == externalPaymentId && p.TenantId == tenantId);
        }
    }
}
