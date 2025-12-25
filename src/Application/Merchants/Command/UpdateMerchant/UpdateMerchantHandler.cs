using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;

namespace PaySplit.Application.Merchants.Command.UpdateMerchant
{
    public class UpdateMerchantHandler : ICommandHandler<UpdateMerchantCommand, Result<UpdateMerchantResult>>
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

            try
            {
                merchant.UpdateDetails(command.Name, command.Email);
                merchant.UpdateRevenueShare(command.RevenueSharePercentage);
            }
            catch (ArgumentException ex)
            {
                return Result<UpdateMerchantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new UpdateMerchantResult(
                merchant.Id,
                merchant.Name,
                merchant.Email,
                merchant.RevenueShare.Value,
                merchant.Status.ToString());

            return Result<UpdateMerchantResult>.Success(result);
        }
    }
}
