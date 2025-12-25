namespace API.Dto.Merchants
{
    public class MerchantResponse
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public decimal RevenueShare { get; set; }
        public string Status { get; set; } = default!;
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? DeactivatedAtUtc { get; set; }
        public DateTimeOffset? SuspendedAtUtc { get; set; }
    }
}
