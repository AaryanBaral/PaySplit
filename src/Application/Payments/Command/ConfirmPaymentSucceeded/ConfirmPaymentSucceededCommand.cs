

using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaymentPlatform.Application.Commands.ConfirmPaymentSucceeded
{
    public class ConfirmPaymentSucceededCommand : ICommand<Result>

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