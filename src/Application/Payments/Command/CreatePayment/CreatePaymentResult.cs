using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace PaySplit.Application.Payments.Command.CreatePayment
{
    public class CreatePaymentResult
    {
        public Guid PaymentId { get; }
        public Guid TenantId { get; }
        public Guid MerchantId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string Status { get; }
        public string ExternalPaymentId { get; }
        public CreatePaymentResult(
    Guid paymentId,
    Guid tenantId,
    Guid merchantId,
    decimal amount,
    string currency,
    string externalPaymentId,
    string status)
        {
            PaymentId = paymentId;
            TenantId = tenantId;
            MerchantId = merchantId;
            Amount = amount;
            Currency = currency;
            Status = status;
            ExternalPaymentId = externalPaymentId;
        }
    }
}