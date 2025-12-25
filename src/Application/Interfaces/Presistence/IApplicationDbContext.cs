using Domain.Merchant;
using Domain.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Presistence
{
    public interface IApplicationDbContext
    {
        DbSet<Tenant> Tenants { get; }
        DbSet<Merchant> Merchants { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}