using Domain.Common;

namespace Domain.Tenants
{
    public class TenantUser : Entity
    {
        public Guid TenantId { get; private set; }
        public string Email { get; private set; } = default!;
        public string DisplayName { get; private set; } = default!;
        public TenantUserRole Role { get; private set; }
        public TenantUserStatus Status { get; private set; }
        public DateTimeOffset JoinedAtUtc { get; private set; }

        // For EF Core
        private TenantUser() { }

        private TenantUser(
            Guid tenantId,
            string email,
            string displayName,
            TenantUserRole role,
            DateTimeOffset joinedAtUtc, TenantUserStatus status)
            : base()
        {
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant id is required.", nameof(tenantId));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name is required.", nameof(displayName));

            TenantId = tenantId;
            Email = email.Trim().ToLowerInvariant();
            DisplayName = displayName.Trim();
            Role = role;
            JoinedAtUtc = joinedAtUtc;
            Status = status;
        }

        // Factory: create a tenant user
        public static TenantUser Create(
            Guid tenantId,
            string email,
            string displayName,
            TenantUserRole role,
            DateTimeOffset joinedAtUtc,
            TenantUserStatus status = TenantUserStatus.Active)
        {
            return new TenantUser(tenantId, email, displayName, role, joinedAtUtc, status);
        }

        public void ChangeRole(TenantUserRole newRole)
        {
            if (Status != TenantUserStatus.Active)
                throw new InvalidOperationException("Cannot change role of an inactive user.");

            Role = newRole;
        }

        public void Deactivate()
        {
            if (Status == TenantUserStatus.Inactive)
                throw new InvalidOperationException("Tenant is already Inactive.");
            Status = TenantUserStatus.Inactive;
        }
        public void Activate()
        {
            if (Status == TenantUserStatus.Active)
                throw new InvalidOperationException("Tenant is already Active.");
            Status = TenantUserStatus.Active;
        }
        public void Suspend()
        {
            if (Status == TenantUserStatus.Suspended)
                throw new InvalidOperationException("Tenant is already Suspended.");
            Status = TenantUserStatus.Suspended;
        }

        public void UpdateProfile(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name is required.", nameof(displayName));

            DisplayName = displayName.Trim();
        }
    }
}
