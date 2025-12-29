using MediatR;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Payments.Command.CreatePayment;

namespace PaySplit.Application.Payments.Command.CreateIncomingPayment
{
    public sealed class CreateIncomingPaymentCommand : IRequest<Result<CreatePaymentResult>>
    {
        public Guid TenantId { get; }
        public Guid MerchantId { get; }
        public string Currency { get; }
        public decimal PaymentAmount { get; }
        public string ExternalPaymentId { get; }

        public CreateIncomingPaymentCommand(
            Guid tenantId,
            Guid merchantId,
            decimal paymentAmount,
            string currency,
            string externalPaymentId)
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            Currency = currency;
            PaymentAmount = paymentAmount;
            ExternalPaymentId = externalPaymentId;
        }
    }
}
