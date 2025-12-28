using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PaySplit.Domain.Ledgers;

namespace PaySplit.Infrastructure.Persistence.Configurations
{
    public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
    {
        public void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.TenantId)
                .IsRequired();

            builder.Property(l => l.Description)
                .IsRequired();

            builder.Property(l => l.SourceId)
                .IsRequired();

            builder.Property(l => l.OccurredAtUtc)
                .IsRequired();

            builder.OwnsOne(l => l.Amount, amount =>
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
