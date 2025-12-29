using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
using PaySplit.Domain.Tenants.Exceptions;

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
            Name = name.Trim();
            DefaultCurrency = defaultCurrency.Trim().ToUpperInvariant();
            CreatedAtUtc = createdAtUtc;
            Status = status;
            ApplyStatusTimestamps(status);
        }

        public static Result<Tenant> Create(string name, string defaultCurrency = "NPR")
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Tenant>.Failure("Name is required.");

            if (string.IsNullOrWhiteSpace(defaultCurrency))
                return Result<Tenant>.Failure("Default currency is required.");

            return Result<Tenant>.Success(
                new Tenant(name, defaultCurrency, DateTimeOffset.UtcNow, TenantStatus.Active));
        }
        public Result Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure("Name is required.");

            Name = newName.Trim();
            return Result.Success();
        }
        public Result Deactivate()
        {
            if (Status == TenantStatus.Inactive)
                return Result.Failure(new TenantAlreadyInactiveException().Message);
            Status = TenantStatus.Inactive;
            DeactivatedAtUtc = DateTimeOffset.UtcNow;
            SuspendedAtUtc = null;
            return Result.Success();
        }
        public Result Activate()
        {
            if (Status == TenantStatus.Active)
                return Result.Failure(new TenantAlreadyActiveException().Message);
            Status = TenantStatus.Active;
            DeactivatedAtUtc = null;
            SuspendedAtUtc = null;
            return Result.Success();
        }
        public Result Suspend()
        {
            if (Status == TenantStatus.Suspended)
                return Result.Failure(new TenantAlreadySuspendedException().Message);
            Status = TenantStatus.Suspended;
            SuspendedAtUtc = DateTimeOffset.UtcNow;
            DeactivatedAtUtc = null;
            return Result.Success();
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
