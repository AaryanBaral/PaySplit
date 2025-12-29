using PaySplit.Application.Common.Models;
using MediatR;

using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Queries;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Payouts;

namespace PaySplit.Application.Payouts.Commands.GeneratePayoutRequest
{
    public class GeneratePayoutRequestHandler: IRequestHandler<GeneratePayoutRequestCommand, Result<GeneratePayoutRequestResult>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IPayoutRepository _payoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMerchantBalanceQuery _merchantBalanceService;

        public GeneratePayoutRequestHandler(ITenantRepository tenantRepository, IMerchantRepository merchantRepository, IPayoutRepository payoutRepository, IUnitOfWork unitOfWork, IMerchantBalanceQuery merchantBalanceService)
        {
            _merchantRepository = merchantRepository;
            _payoutRepository = payoutRepository;
            _tenantRepository = tenantRepository;
            _merchantBalanceService = merchantBalanceService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GeneratePayoutRequestResult>> HandleAsync(GeneratePayoutRequestCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Basic input validation
            if (command.RequestedAmount <= 0)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Requested amount must be positive.");
            }

            if (string.IsNullOrWhiteSpace(command.Currency))
            {
                return Result<GeneratePayoutRequestResult>.Failure("Currency is required.");
            }

            // 2. Ensure tenant exists and is active
            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, cancellationToken);
            if (tenant is null || !tenant.IsActive)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Tenant not found or inactive.");
            }
            if (string.Equals(tenant.DefaultCurrency, command.Currency, StringComparison.OrdinalIgnoreCase))
            {
                return Result<GeneratePayoutRequestResult>.Failure("Currency MissMatch the Tenant doesnot support this currency.");
            }

            // 3. Ensure merchant exists, belongs to tenant, and is active
            var merchant = await _merchantRepository.GetByIdAsync(command.MerchantId, cancellationToken);
            if (merchant is null || merchant.TenantId != command.TenantId)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Merchant not found for this tenant.");
            }

            if (!merchant.IsActive)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Merchant is inactive.");
            }

            // 4. Get merchant balance
            MerchantBalanceDto merchantBalance =
                await _merchantBalanceService.GetAsync(command.TenantId, command.MerchantId, cancellationToken);

            if (merchantBalance.Available.Amount <= 0m)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Merchant has no available balance for payout.");
            }

            // Optional: ensure currency matches balance currency (if you support multiple currencies)
            if (!string.Equals(merchantBalance.Available.Currency, command.Currency, StringComparison.OrdinalIgnoreCase))
            {
                return Result<GeneratePayoutRequestResult>.Failure("Requested currency does not match merchant balance currency.");
            }

            // 5. Ensure requested amount does not exceed available balance
            if (merchantBalance.Available.Amount < command.RequestedAmount)
            {
                return Result<GeneratePayoutRequestResult>.Failure("Requested amount exceeds available balance.");
            }

            // 6. Create payout in domain
            var payoutResult = Payout.Request(
                command.TenantId,
                command.MerchantId,
                command.RequestedAmount,
                command.Currency,
                command.RequestedByUserId,
                DateTimeOffset.UtcNow);

            if (!payoutResult.IsSuccess || payoutResult.Value is null)
            {
                return Result<GeneratePayoutRequestResult>.Failure(
                    payoutResult.Error ?? "Payout request is invalid.");
            }

            // 7. Persist payout
            var payout = payoutResult.Value;
            await _payoutRepository.AddAsync(payout, cancellationToken);

            // 8. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 9. Map to result DTO
            return Result<GeneratePayoutRequestResult>.Success(payout.ToGeneratePayoutRequestResult());
        }

    
        public Task<Result<GeneratePayoutRequestResult>> Handle(GeneratePayoutRequestCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
