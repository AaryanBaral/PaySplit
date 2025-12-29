
using PaySplit.Domain.Common;
using PaySplit.Domain.Common.Results;
using PaySplit.Domain.Merchants.Exceptions;

namespace PaySplit.Domain.Merchants
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
        public bool IsActive { get; private set; }

        private Merchant() { }

        private Merchant(
            Guid tenantId,
            string name,
            string email,
            Percentage revenueShare,
            DateTimeOffset createdAtUtc,
            MerchantStatus status) : base()
        {
            TenantId = tenantId;
            Name = name.Trim();
            RevenueShare = revenueShare;
            Status = status;
            Email = email.Trim().ToLowerInvariant();
            CreatedAtUtc = createdAtUtc;
            ApplyStatusTimestamps(status);
        }
        public static Result<Merchant> Create(
            Guid tenantId,
            string name,
            string email,
            decimal revenueSharePercentage,
            MerchantStatus status = MerchantStatus.Active)
        {
            if (tenantId == Guid.Empty)
                return Result<Merchant>.Failure("Tenant id is required.");

            if (string.IsNullOrWhiteSpace(name))
                return Result<Merchant>.Failure("Name is required.");

            if (string.IsNullOrWhiteSpace(email))
                return Result<Merchant>.Failure("Email is required.");

            var percentageResult = Percentage.Create(revenueSharePercentage);
            if (!percentageResult.IsSuccess || percentageResult.Value is null)
                return Result<Merchant>.Failure(percentageResult.Error ?? "Revenue share percentage is invalid.");

            var merchant = new Merchant(
                tenantId,
                name,
                email,
                percentageResult.Value,
                DateTimeOffset.UtcNow,
                status);

            return Result<Merchant>.Success(merchant);
        }
        public Result UpdateDetails(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure("Name is required.");

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure("Email is required.");

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            return Result.Success();
        }
        public Result UpdateRevenueShare(decimal revenueSharePercentage)
        {
            var percentageResult = Percentage.Create(revenueSharePercentage);
            if (!percentageResult.IsSuccess || percentageResult.Value is null)
                return Result.Failure(percentageResult.Error ?? "Revenue share percentage is invalid.");

            RevenueShare = percentageResult.Value;
            return Result.Success();
        }
        public Result Deactivate()
        {
            if (Status == MerchantStatus.Inactive)
                return Result.Failure(new MerchantInactiveException().Message);
            Status = MerchantStatus.Inactive;
            DeactivatedAtUtc = DateTimeOffset.UtcNow;
            SuspendedAtUtc = null;
            return Result.Success();
        }
        public Result Activate()
        {
            if (Status == MerchantStatus.Active)
                return Result.Failure(new MerchantAlreadyActiveException().Message);
            Status = MerchantStatus.Active;
            DeactivatedAtUtc = null;
            SuspendedAtUtc = null;
            return Result.Success();
        }
        public Result Suspend()
        {
            if (Status == MerchantStatus.Suspended)
                return Result.Failure(new MerchantAlreadySuspendedException().Message);
            Status = MerchantStatus.Suspended;
            SuspendedAtUtc = DateTimeOffset.UtcNow;
            DeactivatedAtUtc = null;
            return Result.Success();
        }

        private void ApplyStatusTimestamps(MerchantStatus status)
        {
            switch (status)
            {
                case MerchantStatus.Active:
                    DeactivatedAtUtc = null;
                    SuspendedAtUtc = null;
                    IsActive = true;
                    break;
                case MerchantStatus.Inactive:
                    DeactivatedAtUtc = DateTimeOffset.UtcNow;
                    SuspendedAtUtc = null;
                    IsActive = false;
                    break;
                case MerchantStatus.Suspended:
                    SuspendedAtUtc = DateTimeOffset.UtcNow;
                    DeactivatedAtUtc = null;
                    IsActive = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), "Invalid merchant status.");
            }
        }
    }
}
