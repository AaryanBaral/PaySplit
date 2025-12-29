
using MediatR;

using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Payments.Command.CreatePayment
{
    public class CreatePaymentCommand : IRequest<Result<CreatePaymentResult>>
    {
        public Guid TenantId { get; }
        public Guid MerchantId { get; }
        public string Currency {get;}
        public decimal PaymentAmount {get;}

        public CreatePaymentCommand(Guid tenantId, Guid merchantId, decimal paymentAmount, string currency)
        {
            TenantId = tenantId;
            MerchantId = merchantId;
            Currency = currency;
            PaymentAmount = paymentAmount;
        }

    }
}
