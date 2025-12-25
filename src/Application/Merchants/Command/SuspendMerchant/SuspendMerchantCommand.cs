using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Merchants.Command.SuspendMerchant
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
