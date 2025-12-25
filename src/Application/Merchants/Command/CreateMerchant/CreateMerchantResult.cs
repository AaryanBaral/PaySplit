using Domain.Merchant;

namespace Application.Merchants.Command.CreateMerchant
{
    public class CreateMerchantResult
    {
        public Guid MerchantId { get; }
        public Guid TenantId { get; }
        public string Status { get; }

        public CreateMerchantResult(Guid merchantId, Guid tenantId, string status)
        {
            MerchantId = merchantId;
            TenantId = tenantId;
            Status = status;
        }
    }
}
