using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
using PaySplit.Domain.Tenants.Exceptions;

namespace PaySplit.Domain.Tenants
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
            TenantId = tenantId;
            Email = email.Trim().ToLowerInvariant();
            DisplayName = displayName.Trim();
            Role = role;
            JoinedAtUtc = joinedAtUtc;
            Status = status;
        }

        // Factory: create a tenant user
        public static Result<TenantUser> Create(
            Guid tenantId,
            string email,
            string displayName,
            TenantUserRole role,
            DateTimeOffset joinedAtUtc,
            TenantUserStatus status = TenantUserStatus.Active)
        {
            if (tenantId == Guid.Empty)
                return Result<TenantUser>.Failure("Tenant id is required.");

            if (string.IsNullOrWhiteSpace(email))
                return Result<TenantUser>.Failure("Email is required.");

            if (string.IsNullOrWhiteSpace(displayName))
                return Result<TenantUser>.Failure("Display name is required.");

            return Result<TenantUser>.Success(
                new TenantUser(tenantId, email, displayName, role, joinedAtUtc, status));
        }

        public Result ChangeRole(TenantUserRole newRole)
        {
            if (Status != TenantUserStatus.Active)
                return Result.Failure(new TenantUserNotActiveException().Message);

            Role = newRole;
            return Result.Success();
        }

        public Result Deactivate()
        {
            if (Status == TenantUserStatus.Inactive)
                return Result.Failure(new TenantUserAlreadyInactiveException().Message);
            Status = TenantUserStatus.Inactive;
            return Result.Success();
        }
        public Result Activate()
        {
            if (Status == TenantUserStatus.Active)
                return Result.Failure(new TenantUserAlreadyActiveException().Message);
            Status = TenantUserStatus.Active;
            return Result.Success();
        }
        public Result Suspend()
        {
            if (Status == TenantUserStatus.Suspended)
                return Result.Failure(new TenantUserAlreadySuspendedException().Message);
            Status = TenantUserStatus.Suspended;
            return Result.Success();
        }

        public Result UpdateProfile(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return Result.Failure("Display name is required.");

            DisplayName = displayName.Trim();
            return Result.Success();
        }
    }
}
