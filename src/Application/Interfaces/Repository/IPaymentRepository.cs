using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PaySplit.Domain.Payments;

namespace PaySplit.Application.Repository
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
        Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}