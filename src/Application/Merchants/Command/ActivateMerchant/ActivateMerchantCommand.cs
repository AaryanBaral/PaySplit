using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Command.ActivateMerchant
{
    public class ActivateMerchantCommand : ICommand<Result<ActivateMerchantResult>>
    {
        public Guid MerchantId { get; }

        public ActivateMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
