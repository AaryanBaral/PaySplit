using Domain.Common;

namespace Domain.Tenants
{
    public class Tenant : Entity
    {
        public string Name { get; protected set; } = default!;
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? DeactivatedAtUtc { get; private set; }
        public DateTimeOffset? SuspendedAtUTC { get; private set; }
        public TenantStatus Status { get; private set; }

        private Tenant() { }

        private Tenant(string name, DateTimeOffset createdAtUtc, TenantStatus status) : base()
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Tenant name is required");
            }
            Name = name.Trim();
            CreatedAtUtc = createdAtUtc;
            Status = status;
            ApplyStatusTimestamps(status);
        }

        public static Tenant Create(string name)
        {
            return new Tenant(name, DateTimeOffset.UtcNow, TenantStatus.Active);
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
            SuspendedAtUTC = null;
        }
        public void Activate()
        {
            if (Status == TenantStatus.Active)
                throw new InvalidOperationException("Tenant is already Active.");
            Status = TenantStatus.Active;
            DeactivatedAtUtc = null;
            SuspendedAtUTC = null;
        }
        public void Suspend()
        {
            if (Status == TenantStatus.Suspended)
                throw new InvalidOperationException("Tenant is already Suspended.");
            Status = TenantStatus.Suspended;
            SuspendedAtUTC = DateTimeOffset.UtcNow;
            DeactivatedAtUtc = null;
        }

        private void ApplyStatusTimestamps(TenantStatus status)
        {
            switch (status)
            {
                case TenantStatus.Active:
                    DeactivatedAtUtc = null;
                    SuspendedAtUTC = null;
                    break;
                case TenantStatus.Inactive:
                    DeactivatedAtUtc = DateTimeOffset.UtcNow;
                    SuspendedAtUTC = null;
                    break;
                case TenantStatus.Suspended:
                    SuspendedAtUTC = DateTimeOffset.UtcNow;
                    DeactivatedAtUtc = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), "Invalid tenant status.");
            }
        }

    }
}
