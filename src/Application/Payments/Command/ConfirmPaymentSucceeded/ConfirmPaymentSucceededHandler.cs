using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Common.Exceptions;
using PaySplit.Domain.Ledgers;
using PaySplit.Domain.Merchants;
using PaySplit.Domain.Payments;
using MediatR;


namespace PaySplit.Application.Payments.Command.ConfirmPaymentSucceeded
{
    public class ConfirmPaymentSucceededHandler : IRequestHandler<ConfirmPaymentSucceededCommand, Result>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly ILedgerEntryRepository _ledgerEntryRepository;
        private readonly IUnitOfWork _unitOfWork;


        public ConfirmPaymentSucceededHandler(
            IPaymentRepository paymentRepository,
            IMerchantRepository merchantRepository,
            ILedgerEntryRepository ledgerEntryRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _merchantRepository = merchantRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
    ConfirmPaymentSucceededCommand command,
    CancellationToken cancellationToken = default)
        {
            Payment? payment = await _paymentRepository.GetByIdAsync(command.PaymentId, cancellationToken);
            if (payment is null)
                return Result.Failure("Payment not found.");

            if (payment.Status != PaymentStatus.Pending)
            {
                return Result.Failure("Only pending payments can be marked as succeeded.");
            }
            try
            {
                payment.MarkSucceeded(command.CompletedAtUtc);
            }
            catch (DomainException ex)
            {
                // Should be rare, but we still turn it into a clean failure
                return Result.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(ex.Message);
            }

            // Load the merchant to get revenue share
            Merchant? merchant = await _merchantRepository.GetByIdAsync(payment.MerchantId, cancellationToken);
            if (merchant is null || !merchant.IsActive)
            {
                // This is a serious data problem, but we'll surface a failure for now.
                return Result.Failure("Merchant not found or inactive for this payment.");
            }


            // calculating the share split
            (PaySplit.Domain.Common.Money merchantAmount, PaySplit.Domain.Common.Money tenantShareAmount) = payment.CalculateRevenueSplit(merchant.RevenueShare);


            // Create ledger entries
            // Merchant credit (they earned money from this payment)
            var merchantCreditEntry = LedgerEntry.CreateMerchantCredit(
                tenantId: payment.TenantId,
                merchantId: payment.MerchantId,
                amount: merchantAmount,
                sourceType: LedgerEntrySourceType.Payment,
                sourceId: payment.Id,
                description: $"Merchant share from payment {payment.Id}",
                occurredAtUtc: command.CompletedAtUtc);

            //  Tenant credit (tenant revenue from this payment)
            var tenantCreditEntry = LedgerEntry.CreateTenantCredit(
                tenantId: payment.TenantId,
                amount: tenantShareAmount,
                sourceType: LedgerEntrySourceType.Payment,
                sourceId: payment.Id,
                description: $"Tenant share from payment {payment.Id}",
                occurredAtUtc: command.CompletedAtUtc);

            //  Persist ledger entries
            await _ledgerEntryRepository.AddAsync(merchantCreditEntry, cancellationToken);
            await _ledgerEntryRepository.AddAsync(tenantCreditEntry, cancellationToken);

            // 8 Save all changes (payment + ledger entries) in a transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    
        public Task<Result> Handle(ConfirmPaymentSucceededCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
