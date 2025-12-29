namespace PaySplit.API.Dto.Merchants
{
    public sealed record UpdateMerchantRequest
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
        public decimal RevenueSharePercentage { get; init; }
    }
}
