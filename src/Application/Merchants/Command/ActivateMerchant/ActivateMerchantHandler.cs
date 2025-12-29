using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Merchants;
using MediatR;

namespace PaySplit.Application.Merchants.Command.ActivateMerchant
{
    public class ActivateMerchantHandler: IRequestHandler<ActivateMerchantCommand, Result<ActivateMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ActivateMerchantResult>> HandleAsync(ActivateMerchantCommand command, CancellationToken cancellationToken = default)
        {
            var merchant = await _repository.GetByIdAsync(command.MerchantId, cancellationToken);
            if (merchant is null)
            {
                return Result<ActivateMerchantResult>.Failure("Merchant id is not valid");
            }

            if (merchant.Status == MerchantStatus.Active)
            {
                return Result<ActivateMerchantResult>.Failure("Merchant is already Activated");
            }

            var activateResult = merchant.Activate();
            if (!activateResult.IsSuccess)
            {
                return Result<ActivateMerchantResult>.Failure(activateResult.Error ?? "Merchant activation failed.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = merchant.ToActivateMerchantResult();
            return Result<ActivateMerchantResult>.Success(result);
        }
    
        public Task<Result<ActivateMerchantResult>> Handle(ActivateMerchantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
