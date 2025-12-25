namespace API.Dto.Merchants
{
    public class UpdateMerchantRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public decimal RevenueSharePercentage { get; set; }
    }
}
