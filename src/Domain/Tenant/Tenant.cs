using Domain.Common;

namespace Domain.Tenant
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
            DeactivatedAtUtc = null;
            SuspendedAtUTC = null;
            Status = status;
        }

        public static Tenant Create(string name, DateTimeOffset createdAtUtc, TenantStatus status = TenantStatus.Active)
        {
            return new Tenant(name, createdAtUtc, status);
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
        }
        public void Activate()
        {
            if (Status == TenantStatus.Active)
                throw new InvalidOperationException("Tenant is already Active.");
            Status = TenantStatus.Active;
        }
        public void Suspend()
        {
            if (Status == TenantStatus.Suspended)
                throw new InvalidOperationException("Tenant is already Suspended.");
            Status = TenantStatus.Suspended;
        }

    }
}