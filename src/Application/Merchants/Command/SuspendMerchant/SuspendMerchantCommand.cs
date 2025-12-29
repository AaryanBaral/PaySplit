using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Command.SuspendMerchant
{
    public class SuspendMerchantCommand : IRequest<Result<SuspendMerchantResult>>
    {
        public Guid MerchantId { get; }

        public SuspendMerchantCommand(Guid merchantId)
        {
            MerchantId = merchantId;
        }
    }
}
