namespace PaySplit.Application.Merchants.Command.ActivateMerchant
{
    public class ActivateMerchantResult
    {
        public Guid MerchantId { get; }
        public string Status { get; }

        public ActivateMerchantResult(Guid merchantId, string status)
        {
            MerchantId = merchantId;
            Status = status;
        }
    }
}
