using MediatR;





using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Payouts.Commands.RejectPayout
{
    public class RejectPayoutCommand : IRequest<Result<RejectPayoutResult>>
    {
        public Guid TenantId { get; }
        public Guid PayoutId { get; }
        public Guid RejectedByUserId { get; }
        public DateTimeOffset RejectedAtUtc { get; }
        public string? Notes { get; }

        public RejectPayoutCommand(
            Guid tenantId,
            Guid payoutId,
            Guid rejectedByUserId,
            DateTimeOffset rejectedAtUtc,
            string? notes)
        {
            TenantId = tenantId;
            PayoutId = payoutId;
            RejectedByUserId = rejectedByUserId;
            RejectedAtUtc = rejectedAtUtc;
            Notes = notes;
        }
    }
}