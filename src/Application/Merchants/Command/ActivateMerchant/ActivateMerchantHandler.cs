using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Merchant;

namespace Application.Merchants.Command.ActivateMerchant
{
    public class ActivateMerchantHandler : ICommandHandler<ActivateMerchantCommand, Result<ActivateMerchantResult>>
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

            try
            {
                merchant.Activate();
            }
            catch (InvalidOperationException ex)
            {
                return Result<ActivateMerchantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new ActivateMerchantResult(merchant.Id, merchant.Status.ToString());
            return Result<ActivateMerchantResult>.Success(result);
        }
    }
}
