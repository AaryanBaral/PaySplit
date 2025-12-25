namespace API.Dto.Tenants
{
    public class TenantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? DeactivatedAtUtc { get; set; }
        public DateTimeOffset? SuspendedAtUtc { get; set; }
    }
}
