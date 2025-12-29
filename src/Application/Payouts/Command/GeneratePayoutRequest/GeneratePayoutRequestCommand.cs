using MediatR;


using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Payouts.Commands.GeneratePayoutRequest
{
    public class GeneratePayoutRequestCommand : IRequest<Result<GeneratePayoutRequestResult>>
    {
        public Guid TenantId { get; }
        public Guid MerchantId { get; }
        public decimal RequestedAmount { get; }
        public string Currency { get; }
        public Guid RequestedByUserId { get; }

        public GeneratePayoutRequestCommand(
            Guid tenantId,
            Guid merchantId,
            decimal requestedAmount,
            string currency,
            Guid requestedByUserId)
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            RequestedAmount = requestedAmount;
            Currency = currency;
            RequestedByUserId = requestedByUserId;
        }

    }
}