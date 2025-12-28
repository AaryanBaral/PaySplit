namespace PaySplit.API.Dto.Tenants
{
    public class CreateTenantRequest
    {
        public string Name { get; set; } = default!;
        public string? DefaultCurrency { get; set; }
    }
}
