using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Merchants;

using MediatR;

namespace PaySplit.Application.Merchants.Command.SuspendMerchant
{
    public class SuspendMerchantHandler : IRequestHandler<SuspendMerchantCommand, Result<SuspendMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SuspendMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SuspendMerchantResult>> Handle(SuspendMerchantCommand command, CancellationToken cancellationToken = default)
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

            var suspendResult = merchant.Suspend();
            if (!suspendResult.IsSuccess)
            {
                return Result<SuspendMerchantResult>.Failure(suspendResult.Error ?? "Merchant suspension failed.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = merchant.ToSuspendMerchantResult();
            return Result<SuspendMerchantResult>.Success(result);
        }

    }
}
