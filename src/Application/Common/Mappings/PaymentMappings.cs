using PaySplit.Application.Payments.Command.CreatePayment;
using PaySplit.Domain.Payments;

namespace PaySplit.Application.Common.Mappings
{
    public static class PaymentMappings
    {
    public static CreatePaymentResult ToCreatePaymentResult(this Payment payment) =>
        new(
            paymentId: payment.Id,
            tenantId: payment.TenantId,
            merchantId: payment.MerchantId,
            amount: payment.Amount.Amount,
            currency: payment.Amount.Currency,
            externalPaymentId: payment.ExternalPaymentId,
            status: payment.Status.ToString()
        );
    }
}
