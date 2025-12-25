
namespace PaySplit.Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Status { get; init; } = default!;
        public DateTimeOffset CreatedAtUtc { get; init; }
        public DateTimeOffset? DeactivatedAtUtc { get; init; }
        public DateTimeOffset? SuspendedAtUtc { get; init; }

        public GetTenantByIdDto(
            Guid id,
            string name,
            string status,
            DateTimeOffset createdAtUtc,
            DateTimeOffset? deactivatedAtUtc,
            DateTimeOffset? suspendedAtUtc)
        {
            Id = id;
            Name = name;
            Status = status;
            CreatedAtUtc = createdAtUtc;
            DeactivatedAtUtc = deactivatedAtUtc;
            SuspendedAtUtc = suspendedAtUtc;
        }
    }
}
