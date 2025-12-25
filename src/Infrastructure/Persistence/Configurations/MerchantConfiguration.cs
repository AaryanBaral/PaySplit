using Domain.Merchant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired();

            builder.Property(m => m.Email)
                .IsRequired();

            builder.Property(m => m.TenantId)
                .IsRequired();

            builder.OwnsOne(m => m.RevenueShare, revenueShare =>
            {
                revenueShare.Property(p => p.Value)
                    .HasColumnName("RevenueShare")
                    .HasPrecision(5, 2);
            });
        }
    }
}
