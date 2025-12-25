namespace Application.Merchants.Command.UpdateMerchant
{
    public class UpdateMerchantResult
    {
        public Guid MerchantId { get; }
        public string Name { get; }
        public string Email { get; }
        public decimal RevenueSharePercentage { get; }
        public string Status { get; }

        public UpdateMerchantResult(Guid merchantId, string name, string email, decimal revenueSharePercentage, string status)
        {
            MerchantId = merchantId;
            Name = name;
            Email = email;
            RevenueSharePercentage = revenueSharePercentage;
            Status = status;
        }
    }
}
