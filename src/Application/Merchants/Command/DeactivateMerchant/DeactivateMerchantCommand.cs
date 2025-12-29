using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Command.DeactivateMerchant
{
    public class DeactivateMerchantCommand : IRequest<Result<DeactivateMerchantResult>>
    {
        public Guid MerchantId { get; }

        public DeactivateMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
