

namespace PaySplit.Application.Payouts.Commands.CompletePayout
{
    public class CompletePayoutResult
    {
        public Guid PayoutId { get; }
        public string Status { get; }

        public CompletePayoutResult(Guid payoutId, string status)
        {
            PayoutId = payoutId;
            Status = status;
        }
    }
}