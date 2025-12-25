namespace Application.Merchants.Command.SuspendMerchant
{
    public class SuspendMerchantResult
    {
        public Guid MerchantId { get; }
        public string Status { get; }

        public SuspendMerchantResult(Guid merchantId, string status)
        {
            MerchantId = merchantId;
            Status = status;
        }
    }
}
