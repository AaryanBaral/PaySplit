using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Merchants.Command.ActivateMerchant
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
