namespace PaySplit.API.Dto.Merchants
{
    public class CreateMerchantRequest
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public decimal RevenueSharePercentage { get; set; }
    }
}
