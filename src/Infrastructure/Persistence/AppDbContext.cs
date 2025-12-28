using Microsoft.EntityFrameworkCore;

using PaySplit.Domain.Ledgers;
using PaySplit.Domain.Merchants;
using PaySplit.Domain.Payments;
using PaySplit.Domain.Payouts;
using PaySplit.Domain.Tenants;

namespace PaySplit.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<TenantUser> TenantUsers => Set<TenantUser>();
        public DbSet<Merchant> Merchants => Set<Merchant>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Payout> Payouts => Set<Payout>();
        public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply all IEntityTypeConfiguration<T> in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
