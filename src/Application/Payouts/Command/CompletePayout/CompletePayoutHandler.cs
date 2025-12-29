using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Ledgers;
using PaySplit.Domain.Payouts;
using MediatR;

namespace PaySplit.Application.Payouts.Commands.CompletePayout
{
    public class CompletePayoutHandler : IRequestHandler<CompletePayoutCommand, Result<CompletePayoutResult>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IPayoutRepository _payoutRepository;
        private readonly ILedgerEntryRepository _ledgerEntryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompletePayoutHandler(
            ITenantRepository tenantRepository,
            IPayoutRepository payoutRepository,
            ILedgerEntryRepository ledgerEntryRepository,
            IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _payoutRepository = payoutRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompletePayoutResult>> HandleAsync(
            CompletePayoutCommand command,
            CancellationToken cancellationToken = default)
        {
            // 1. Check tenant
            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, cancellationToken);
            if (tenant is null || !tenant.IsActive)
            {
                return Result<CompletePayoutResult>.Failure("Tenant not found or inactive.");
            }

            // 2. Load payout
            var payout = await _payoutRepository.GetByIdAsync(command.PayoutId, cancellationToken);
            if (payout is null || payout.TenantId != command.TenantId)
            {
                return Result<CompletePayoutResult>.Failure("Payout not found for this tenant.");
            }

            // 3. Only approved payouts can be completed
            if (payout.Status != PayoutStatus.Approved)
            {
                return Result<CompletePayoutResult>.Failure("Only approved payouts can be completed.");
            }

            // 4. Domain change: mark as completed
            var completeResult = payout.MarkCompleted(
                command.CompletedByUserId,
                command.CompletedAtUtc,
                command.Reference);

            if (!completeResult.IsSuccess)
            {
                return Result<CompletePayoutResult>.Failure(
                    completeResult.Error ?? "Payout completion failed.");
            }

            // 5. Create ledger entry: merchant debit (their balance goes down)
            var merchantDebitResult = LedgerEntry.CreateMerchantDebit(
                tenantId: payout.TenantId,
                merchantId: payout.MerchantId,
                amount: payout.Amount,
                sourceType: LedgerEntrySourceType.Payout,
                sourceId: payout.Id,
                description: $"Payout {payout.Id} completed",
                occurredAtUtc: command.CompletedAtUtc);

            if (!merchantDebitResult.IsSuccess || merchantDebitResult.Value is null)
            {
                return Result<CompletePayoutResult>.Failure(
                    merchantDebitResult.Error ?? "Ledger entry failed.");
            }

            await _ledgerEntryRepository.AddAsync(merchantDebitResult.Value, cancellationToken);

            // 6. Save all changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Map to result
            return Result<CompletePayoutResult>.Success(payout.ToCompletePayoutResult());
        }

        public Task<Result<CompletePayoutResult>> Handle(CompletePayoutCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
