using PaySplit.Domain.Common;

namespace PaySplit.Domain.Tenants
{
    public class Tenant : Entity
    {
        public string Name { get; protected set; } = default!;
        public string DefaultCurrency { get; private set; } = default!;
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? DeactivatedAtUtc { get; private set; }
        public DateTimeOffset? SuspendedAtUtc { get; private set; }
        public TenantStatus Status { get; private set; }
        public bool IsActive { get; private set; }

        private Tenant() { }

        private Tenant(string name, string defaultCurrency, DateTimeOffset createdAtUtc, TenantStatus status) : base()
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Tenant name is required");
            }
            if (string.IsNullOrWhiteSpace(defaultCurrency))
            {
                throw new ArgumentException("Default currency is required.", nameof(defaultCurrency));
            }
            Name = name.Trim();
            DefaultCurrency = defaultCurrency.Trim().ToUpperInvariant();
            CreatedAtUtc = createdAtUtc;
            Status = status;
            ApplyStatusTimestamps(status);
        }

        public static Tenant Create(string name, string defaultCurrency = "NPR")
        {
            return new Tenant(name, defaultCurrency, DateTimeOffset.UtcNow, TenantStatus.Active);
        }
        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Tenant name is required.", nameof(newName));

            Name = newName.Trim();
        }
        public void Deactivate()
        {
            if (Status == TenantStatus.Inactive)
                throw new InvalidOperationException("Tenant is already Inactive.");
            Status = TenantStatus.Inactive;
            DeactivatedAtUtc = DateTimeOffset.UtcNow;
            SuspendedAtUtc = null;
        }
        public void Activate()
        {
            if (Status == TenantStatus.Active)
                throw new InvalidOperationException("Tenant is already Active.");
            Status = TenantStatus.Active;
            DeactivatedAtUtc = null;
            SuspendedAtUtc = null;
        }
        public void Suspend()
        {
            if (Status == TenantStatus.Suspended)
                throw new InvalidOperationException("Tenant is already Suspended.");
            Status = TenantStatus.Suspended;
            SuspendedAtUtc = DateTimeOffset.UtcNow;
            DeactivatedAtUtc = null;
        }

        private void ApplyStatusTimestamps(TenantStatus status)
        {
            switch (status)
            {
                case TenantStatus.Active:
                    DeactivatedAtUtc = null;
                    SuspendedAtUtc = null;
                    IsActive = true;
                    break;
                case TenantStatus.Inactive:
                    DeactivatedAtUtc = DateTimeOffset.UtcNow;
                    SuspendedAtUtc = null;
                    IsActive = false;
                    break;
                case TenantStatus.Suspended:
                    SuspendedAtUtc = DateTimeOffset.UtcNow;
                    DeactivatedAtUtc = null;
                    IsActive = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), "Invalid tenant status.");
            }
        }

    }
}
