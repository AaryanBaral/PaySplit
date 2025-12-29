using PaySplit.Domain.Payments;

namespace PaySplit.Application.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Payment?> GetByExternalIdAsync(Guid tenantId, string externalPaymentId, CancellationToken cancellationToken = default);
    }
}
