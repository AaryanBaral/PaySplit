using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaySplit.Domain.Tenants;

namespace PaySplit.Infrastructure.Persistence.Configurations
{
    public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
    {
        public void Configure(EntityTypeBuilder<TenantUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.TenantId)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.DisplayName)
                .IsRequired();

            builder.Property(u => u.JoinedAtUtc)
                .IsRequired();
        }
    }
}
