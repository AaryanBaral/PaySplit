

namespace PaySplit.Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Status { get; init; } = default!;
        public DateTimeOffset CreatedAtUtc { get; init; }

        public GetAllTenantDto(Guid id, string name, string status, DateTimeOffset createdAtUtc)
        {
            Id = id;
            Name = name;
            Status = status;
            CreatedAtUtc = createdAtUtc;
        }
    }
}
