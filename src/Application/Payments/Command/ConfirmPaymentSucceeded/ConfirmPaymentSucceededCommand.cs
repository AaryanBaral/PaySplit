using MediatR;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Payments.Command.ConfirmPaymentSucceeded
{
    public class ConfirmPaymentSucceededCommand : IRequest<Result>

    {
        public Guid PaymentId { get; }
        public DateTimeOffset CompletedAtUtc { get; }

        public ConfirmPaymentSucceededCommand(Guid paymentId, DateTimeOffset completedAtUtc)
        {
            PaymentId = paymentId;
            CompletedAtUtc = completedAtUtc;
        }
    }
}
