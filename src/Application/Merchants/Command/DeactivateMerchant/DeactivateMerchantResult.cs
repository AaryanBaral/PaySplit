namespace PaySplit.Application.Merchants.Command.DeactivateMerchant
{
    public class DeactivateMerchantResult
    {
        public Guid MerchantId { get; }
        public string Status { get; }

        public DeactivateMerchantResult(Guid merchantId, string status)
        {
            MerchantId = merchantId;
            Status = status;
        }
    }
}
