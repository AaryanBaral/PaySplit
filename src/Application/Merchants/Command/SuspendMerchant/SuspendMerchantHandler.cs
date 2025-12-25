using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Merchant;

namespace Application.Merchants.Command.SuspendMerchant
{
    public class SuspendMerchantHandler : ICommandHandler<SuspendMerchantCommand, Result<SuspendMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SuspendMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SuspendMerchantResult>> HandleAsync(SuspendMerchantCommand command, CancellationToken cancellationToken = default)
        {
            var merchant = await _repository.GetByIdAsync(command.MerchantId, cancellationToken);
            if (merchant is null)
            {
                return Result<SuspendMerchantResult>.Failure("Merchant id is not valid");
            }

            if (merchant.Status == MerchantStatus.Suspended)
            {
                return Result<SuspendMerchantResult>.Failure("Merchant is already Suspended");
            }

            try
            {
                merchant.Suspend();
            }
            catch (InvalidOperationException ex)
            {
                return Result<SuspendMerchantResult>.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new SuspendMerchantResult(merchant.Id, merchant.Status.ToString());
            return Result<SuspendMerchantResult>.Success(result);
        }
    }
}
