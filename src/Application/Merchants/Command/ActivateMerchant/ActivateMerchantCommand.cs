using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Command.ActivateMerchant
{
    public class ActivateMerchantCommand : IRequest<Result<ActivateMerchantResult>>
    {
        public Guid MerchantId { get; }

        public ActivateMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
