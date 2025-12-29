namespace PaySplit.API.Dto.Tenants
{
    public sealed record CreateTenantRequest
    {
        public required string Name { get; init; }
        public string? DefaultCurrency { get; init; }
    }
}
