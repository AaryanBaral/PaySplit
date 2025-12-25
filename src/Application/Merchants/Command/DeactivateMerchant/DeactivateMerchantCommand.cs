using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Command.DeactivateMerchant
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
