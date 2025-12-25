using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Merchants.Command.DeactivateMerchant
{
    public class DeactivateMerchantCommand : ICommand<Result<DeactivateMerchantResult>>
    {
        public Guid MerchantId { get; }

        public DeactivateMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
