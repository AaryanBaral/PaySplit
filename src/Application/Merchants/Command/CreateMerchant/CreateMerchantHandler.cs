using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Application.Interfaces.Repository;
using Domain.Merchant;

namespace Application.Merchants.Command.CreateMerchant
{
    public class CreateMerchantHandler : ICommandHandler<CreateMerchantCommand, Result<CreateMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateMerchantResult>> HandleAsync(CreateMerchantCommand command, CancellationToken cancellationToken = default)
        {
            if (command.TenantId == Guid.Empty)
            {
                return Result<CreateMerchantResult>.Failure("Tenant id is required.");
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<CreateMerchantResult>.Failure("Merchant name is required.");
            }

            if (string.IsNullOrWhiteSpace(command.Email))
            {
                return Result<CreateMerchantResult>.Failure("Merchant email is required.");
            }

            if (command.RevenueSharePercentage <= 0 || command.RevenueSharePercentage >= 100)
            {
                return Result<CreateMerchantResult>.Failure("Revenue share percentage must be between 0 and 100 (exclusive).");
            }

            Merchant merchant;
            try
            {
                merchant = Merchant.Create(
                    command.TenantId,
                    command.Name,
                    command.Email,
                    command.RevenueSharePercentage);
            }
            catch (ArgumentException ex)
            {
                return Result<CreateMerchantResult>.Failure(ex.Message);
            }

            await _repository.AddAsync(merchant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new CreateMerchantResult(
                merchant.Id,
                merchant.TenantId,
                merchant.Status.ToString());

            return Result<CreateMerchantResult>.Success(result);
        }
    }
}
