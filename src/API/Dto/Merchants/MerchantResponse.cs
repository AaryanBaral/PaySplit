namespace PaySplit.API.Dto.Merchants
{
    public sealed record MerchantResponse
    {
        public Guid Id { get; init; }
        public Guid TenantId { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public decimal RevenueShare { get; init; }
        public required string Status { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
        public DateTimeOffset? DeactivatedAtUtc { get; init; }
        public DateTimeOffset? SuspendedAtUtc { get; init; }
    }
}
