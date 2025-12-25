using Application.Interfaces.Presistence;
using Domain.Merchant;
using Domain.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<TenantUser> TenantUsers => Set<TenantUser>();
        public DbSet<Merchant> Merchants => Set<Merchant>();


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This is the IUnitOfWork implementation
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
