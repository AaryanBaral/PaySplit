using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Merchants;

namespace PaySplit.Application.Merchants.Command.DeactivateMerchant
{
    public class DeactivateMerchantHandler : ICommandHandler<DeactivateMerchantCommand, Result<DeactivateMerchantResult>>
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

            try
            {
                merchant.Deactivate();
            }
            catch (InvalidOperationException ex)
            {
                return Result<DeactivateMerchantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new DeactivateMerchantResult(merchant.Id, merchant.Status.ToString());
            return Result<DeactivateMerchantResult>.Success(result);
        }
    }
}
