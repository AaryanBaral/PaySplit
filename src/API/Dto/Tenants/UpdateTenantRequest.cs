namespace PaySplit.API.Dto.Tenants
{
    public sealed record UpdateTenantRequest
    {
        public required string Name { get; init; }
    }
}
