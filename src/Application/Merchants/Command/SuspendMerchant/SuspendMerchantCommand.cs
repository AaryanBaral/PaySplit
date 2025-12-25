using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Command.SuspendMerchant
{
    public class SuspendMerchantCommand : ICommand<Result<SuspendMerchantResult>>
    {
        public Guid MerchantId { get; }

        public SuspendMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
