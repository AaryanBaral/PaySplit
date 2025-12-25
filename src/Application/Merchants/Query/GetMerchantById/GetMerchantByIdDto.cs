namespace PaySplit.Application.Merchants.Query.GetMerchantById
{
    public class GetMerchantByIdDto
    {
        public Guid Id { get; }
        public Guid TenantId { get; }
        public string Name { get; }
        public string Email { get; }
        public decimal RevenueShare { get; }
        public string Status { get; }
        public DateTimeOffset CreatedAtUtc { get; }
        public DateTimeOffset? DeactivatedAtUtc { get; }
        public DateTimeOffset? SuspendedAtUtc { get; }

        public GetMerchantByIdDto(
            Guid id,
            Guid tenantId,
            string name,
            string email,
            decimal revenueShare,
            string status,
            DateTimeOffset createdAtUtc,
            DateTimeOffset? deactivatedAtUtc,
            DateTimeOffset? suspendedAtUtc)
        {
            Id = id;
            TenantId = tenantId;
            Name = name;
            Email = email;
            RevenueShare = revenueShare;
            Status = status;
            CreatedAtUtc = createdAtUtc;
            DeactivatedAtUtc = deactivatedAtUtc;
            SuspendedAtUtc = suspendedAtUtc;
        }
    }
}
