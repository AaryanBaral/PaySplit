using PaySplit.Application.Payouts.Commands.ApprovePayout;
using PaySplit.Application.Payouts.Commands.CompletePayout;
using PaySplit.Application.Payouts.Commands.GeneratePayoutRequest;
using PaySplit.Application.Payouts.Commands.RejectPayout;
using PaySplit.Domain.Payouts;

namespace PaySplit.Application.Common.Mappings
{
    public static class PayoutMappings
    {
        public static GeneratePayoutRequestResult ToGeneratePayoutRequestResult(this Payout payout) =>
            new(
                payoutId: payout.Id,
                tenantId: payout.TenantId,
                merchantId: payout.MerchantId,
                amount: payout.Amount.Amount,
                currency: payout.Amount.Currency,
                status: payout.Status.ToString()
            );

        public static ApprovePayoutResult ToApprovePayoutResult(this Payout payout) =>
            new(payout.Id, payout.Status.ToString());

        public static RejectPayoutResult ToRejectPayoutResult(this Payout payout) =>
            new(
                payoutId: payout.Id,
                tenantId: payout.TenantId,
                merchantId: payout.MerchantId,
                amount: payout.Amount.Amount,
                currency: payout.Amount.Currency,
                status: payout.Status.ToString(),
                notes: payout.Notes
            );

        public static CompletePayoutResult ToCompletePayoutResult(this Payout payout) =>
            new(payout.Id, payout.Status.ToString());
    }
}
