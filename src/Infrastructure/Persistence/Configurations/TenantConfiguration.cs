using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaySplit.Domain.Tenants;

namespace PaySplit.Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired();

            builder.Property(t => t.DefaultCurrency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(t => t.CreatedAtUtc)
                .IsRequired();
        }
    }
}
