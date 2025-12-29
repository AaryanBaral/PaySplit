using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Merchants.Command.UpdateMerchant
{
    public class UpdateMerchantHandler: IRequestHandler<UpdateMerchantCommand, Result<UpdateMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateMerchantResult>> HandleAsync(UpdateMerchantCommand command, CancellationToken cancellationToken = default)
        {
            if (command.Id == Guid.Empty)
            {
                return Result<UpdateMerchantResult>.Failure("Merchant id is required.");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<UpdateMerchantResult>.Failure("Merchant name is required.");
            }

            if (string.IsNullOrWhiteSpace(command.Email))
            {
                return Result<UpdateMerchantResult>.Failure("Merchant email is required.");
            }

            if (command.RevenueSharePercentage <= 0 || command.RevenueSharePercentage >= 100)
            {
                return Result<UpdateMerchantResult>.Failure("Revenue share percentage must be between 0 and 100 (exclusive).");
            }

            var merchant = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (merchant is null)
            {
                return Result<UpdateMerchantResult>.Failure("Merchant not found");
            }

            var detailsResult = merchant.UpdateDetails(command.Name, command.Email);
            if (!detailsResult.IsSuccess)
            {
                return Result<UpdateMerchantResult>.Failure(detailsResult.Error ?? "Merchant details are invalid.");
            }

            var shareResult = merchant.UpdateRevenueShare(command.RevenueSharePercentage);
            if (!shareResult.IsSuccess)
            {
                return Result<UpdateMerchantResult>.Failure(shareResult.Error ?? "Revenue share is invalid.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = merchant.ToUpdateMerchantResult();

            return Result<UpdateMerchantResult>.Success(result);
        }
    
        public Task<Result<UpdateMerchantResult>> Handle(UpdateMerchantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
