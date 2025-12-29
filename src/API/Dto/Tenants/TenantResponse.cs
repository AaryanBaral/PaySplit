namespace PaySplit.API.Dto.Tenants
{
    public sealed record TenantResponse
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Status { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
        public DateTimeOffset? DeactivatedAtUtc { get; init; }
        public DateTimeOffset? SuspendedAtUtc { get; init; }
    }
}
