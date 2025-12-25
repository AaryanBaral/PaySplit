
using Domain.Common;

namespace Domain.Merchant
{
    public class Merchant : Entity
    {
        public Guid TenantId { get; protected set; }
        public string Name { get; protected set; } = default!;
        public string Email { get; protected set; } = default!;
        public Percentage RevenueShare { get; protected set; } = default!;
        public MerchantStatus Status { get; protected set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? DeactivatedAtUtc { get; private set; }
        public DateTimeOffset? SuspendedAtUtc { get; private set; }

        private Merchant() { }

        private Merchant(
            Guid tenantId,
            string name,
            string email,
            Percentage revenueShare,
            DateTimeOffset createdAtUtc,
            MerchantStatus status) : base()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Merchant name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Merchant email is required.", nameof(email));
            TenantId = tenantId;
            Name = name.Trim();
            RevenueShare = revenueShare;
            Status = status;
            Email = email.Trim().ToLowerInvariant();
            CreatedAtUtc = createdAtUtc;
            DeactivatedAtUtc = null;
            SuspendedAtUtc = null;
        }
        public static Merchant Create(
            Guid tenantId,
            string name,
            string email,
            decimal revenueSharePercentage,
            MerchantStatus status = MerchantStatus.Active)
        {
            var percentage = Percentage.Create(revenueSharePercentage);
            return new Merchant(tenantId, name, email, percentage, DateTimeOffset.UtcNow, status);
        }
        public void UpdateDetails(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Merchant name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Merchant email is required.", nameof(email));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
        }
        public void UpdateRevenueShare(decimal revenueSharePercentage)
        {
            RevenueShare = Percentage.Create(revenueSharePercentage);
        }
        public void Deactivate()
        {
            if (Status == MerchantStatus.Inactive)
                throw new InvalidOperationException("Merchant is already Inactive.");
            Status = MerchantStatus.Inactive;
            DeactivatedAtUtc = DateTimeOffset.UtcNow;
            SuspendedAtUtc = null;
        }
        public void Activate()
        {
            if (Status == MerchantStatus.Active)
                throw new InvalidOperationException("Merchant is already Active.");
            Status = MerchantStatus.Active;
            DeactivatedAtUtc = null;
            SuspendedAtUtc = null;
        }
        public void Suspend()
        {
            if (Status == MerchantStatus.Suspended)
                throw new InvalidOperationException("Merchant is already Suspended.");
            Status = MerchantStatus.Suspended;
            SuspendedAtUtc = DateTimeOffset.UtcNow;
        }

    }
}
