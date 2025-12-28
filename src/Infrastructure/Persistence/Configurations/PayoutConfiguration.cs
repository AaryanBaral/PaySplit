using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaySplit.Domain.Payouts;

namespace PaySplit.Infrastructure.Persistence.Configurations
{
    public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
    {
        public void Configure(EntityTypeBuilder<Payout> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TenantId)
                .IsRequired();

            builder.Property(p => p.MerchantId)
                .IsRequired();

            builder.Property(p => p.RequestedByUserId)
                .IsRequired();

            builder.Property(p => p.RequestedAtUtc)
                .IsRequired();

            builder.OwnsOne(p => p.Amount, amount =>
            {
                amount.Property(a => a.Currency)
                    .HasMaxLength(3)
                    .IsRequired();

                amount.Property(a => a.Amount)
                    .HasPrecision(18, 2)
                    .IsRequired();
            });
        }
    }
}
