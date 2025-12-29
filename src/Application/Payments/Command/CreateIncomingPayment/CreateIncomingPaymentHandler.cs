using MediatR;
using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Application.Payments.Command.CreatePayment;
using PaySplit.Domain.Payments;

namespace PaySplit.Application.Payments.Command.CreateIncomingPayment
{
    public sealed class CreateIncomingPaymentHandler : IRequestHandler<CreateIncomingPaymentCommand, Result<CreatePaymentResult>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateIncomingPaymentHandler(
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

        public async Task<Result<CreatePaymentResult>> Handle(CreateIncomingPaymentCommand command, CancellationToken ct)
        {
            if (command.TenantId == Guid.Empty) return Result<CreatePaymentResult>.Failure("tenantId is required");
            if (command.MerchantId == Guid.Empty) return Result<CreatePaymentResult>.Failure("merchantId is required");
            if (command.PaymentAmount <= 0m) return Result<CreatePaymentResult>.Failure("paymentAmount must be > 0");
            if (string.IsNullOrWhiteSpace(command.Currency) || command.Currency.Length != 3)
                return Result<CreatePaymentResult>.Failure("currency must be a 3-letter ISO code");
            if (string.IsNullOrWhiteSpace(command.ExternalPaymentId))
                return Result<CreatePaymentResult>.Failure("externalPaymentId is required");

            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, ct);
            if (tenant is null) return Result<CreatePaymentResult>.Failure("tenant not found");
            if (!tenant.IsActive) return Result<CreatePaymentResult>.Failure("tenant inactive");

            var merchant = await _merchantRepository.GetByIdAsync(command.MerchantId, ct);
            if (merchant is null) return Result<CreatePaymentResult>.Failure("merchant not found");
            if (!merchant.IsActive) return Result<CreatePaymentResult>.Failure("merchant inactive");

            var existing = await _paymentRepository.GetByExternalIdAsync(
                command.TenantId, command.ExternalPaymentId.Trim(), ct);

            if (existing is not null)
            {
                return Result<CreatePaymentResult>.Success(existing.ToCreatePaymentResult());
            }

            var paymentResult = Payment.CreateIncoming(
                command.TenantId,
                command.MerchantId,
                command.PaymentAmount,
                command.Currency.ToUpperInvariant(),
                command.ExternalPaymentId.Trim());

            if (!paymentResult.IsSuccess || paymentResult.Value is null)
            {
                return Result<CreatePaymentResult>.Failure(paymentResult.Error ?? "Payment is invalid.");
            }

            var payment = paymentResult.Value;
            await _paymentRepository.AddAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<CreatePaymentResult>.Success(payment.ToCreatePaymentResult());
        }
    }
}
