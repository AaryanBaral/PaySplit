using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaySplit.Domain.Payments;

namespace PaySplit.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.TenantId)
                .IsRequired();

            builder.Property(p => p.MerchantId)
                .IsRequired();

            builder.Property(p => p.ExternalPaymentId)
                .IsRequired();

            builder.Property(p => p.CreatedAtUtc)
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
