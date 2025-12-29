using MediatR;
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Common.Exceptions;
using PaySplit.Domain.Payments;

namespace PaySplit.Application.Payments.Command.CreatePayment
{
    public sealed class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Result<CreatePaymentResult>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentHandler(
            ITenantRepository tenantRepository,
            IMerchantRepository merchantRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _merchantRepository = merchantRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreatePaymentResult>> Handle(CreatePaymentCommand command, CancellationToken ct)
        {
            // Validate input (fast fail)
            if (command.TenantId == Guid.Empty) return Result<CreatePaymentResult>.Failure("tenantId is required");
            if (command.MerchantId == Guid.Empty) return Result<CreatePaymentResult>.Failure("merchantId is required");
            if (command.PaymentAmount <= 0m) return Result<CreatePaymentResult>.Failure("paymentAmount must be > 0");
            if (string.IsNullOrWhiteSpace(command.Currency) || command.Currency.Length != 3)
                return Result<CreatePaymentResult>.Failure("currency must be a 3-letter ISO code");

            // Fetch tenant/merchant
            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, ct);
            if (tenant is null) return Result<CreatePaymentResult>.Failure("tenant not found");
            if (!tenant.IsActive) return Result<CreatePaymentResult>.Failure("tenant inactive");

            var merchant = await _merchantRepository.GetByIdAsync(command.MerchantId, ct);
            if (merchant is null) return Result<CreatePaymentResult>.Failure("merchant not found");
            if (!merchant.IsActive) return Result<CreatePaymentResult>.Failure("merchant inactive");

            // Idempotency: if external id provided, avoid duplicates
            if (!string.IsNullOrWhiteSpace(command.ExternalPaymentId))
            {
                var existing = await _paymentRepository.GetByExternalIdAsync(
                    command.TenantId, command.ExternalPaymentId, ct);

                if (existing is not null)
                {
                    return Result<CreatePaymentResult>.Success(existing.ToCreatePaymentResult());
                }
            }

            Payment payment;
            try
            {
                payment = Payment.CreatePending(
                    command.TenantId,
                    command.MerchantId,
                    command.PaymentAmount,
                    command.Currency.ToUpperInvariant(),
                    command.ExternalPaymentId?.Trim() ?? string.Empty
                );
            }
            catch (DomainException ex)
            {
                return Result<CreatePaymentResult>.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<CreatePaymentResult>.Failure(ex.Message);
            }

            await _paymentRepository.AddAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<CreatePaymentResult>.Success(payment.ToCreatePaymentResult());
        }
    }
}
