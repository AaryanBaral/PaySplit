using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Merchants;
using MediatR;

namespace PaySplit.Application.Merchants.Command.DeactivateMerchant
{
    public class DeactivateMerchantHandler: IRequestHandler<DeactivateMerchantCommand, Result<DeactivateMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeactivateMerchantResult>> HandleAsync(DeactivateMerchantCommand command, CancellationToken cancellationToken = default)
        {
            var merchant = await _repository.GetByIdAsync(command.MerchantId, cancellationToken);
            if (merchant is null)
            {
                return Result<DeactivateMerchantResult>.Failure("Merchant id is not valid");
            }

            if (merchant.Status == MerchantStatus.Inactive)
            {
                return Result<DeactivateMerchantResult>.Failure("Merchant is already Deactivated");
            }

            var deactivateResult = merchant.Deactivate();
            if (!deactivateResult.IsSuccess)
            {
                return Result<DeactivateMerchantResult>.Failure(deactivateResult.Error ?? "Merchant deactivation failed.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = merchant.ToDeactivateMerchantResult();
            return Result<DeactivateMerchantResult>.Success(result);
        }
    
        public Task<Result<DeactivateMerchantResult>> Handle(DeactivateMerchantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
