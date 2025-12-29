using Microsoft.Extensions.Logging;
using MediatR;

using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Persistence;
using PaySplit.Application.Interfaces.Repository;
using PaySplit.Domain.Common.Exceptions;
using PaySplit.Domain.Merchants;

namespace PaySplit.Application.Merchants.Command.CreateMerchant
{
    public class CreateMerchantHandler: IRequestHandler<CreateMerchantCommand, Result<CreateMerchantResult>>
    {
        private readonly IMerchantRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateMerchantHandler> _logger;

        public CreateMerchantHandler(IMerchantRepository repository, IUnitOfWork unitOfWork, ILogger<CreateMerchantHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CreateMerchantResult>> HandleAsync(CreateMerchantCommand command, CancellationToken cancellationToken = default)
        {
            Merchant merchant;
            try
            {
                _logger.LogInformation(
                    "Creating merchant for tenant {TenantId} with revenue share {RevenueSharePercentage}",
                    command.TenantId,
                    command.RevenueSharePercentage);
                merchant = Merchant.Create(
                    command.TenantId,
                    command.Name,
                    command.Email,
                    command.RevenueSharePercentage);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning("Create merchant failed: {Error}", ex.Message);
                return Result<CreateMerchantResult>.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Create merchant failed: {Error}", ex.Message);
                return Result<CreateMerchantResult>.Failure(ex.Message);
            }

            await _repository.AddAsync(merchant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = merchant.ToCreateMerchantResult();

            _logger.LogInformation("Merchant created {MerchantId} with status {Status}", merchant.Id, merchant.Status);
            return Result<CreateMerchantResult>.Success(result);
        }
    
        public Task<Result<CreateMerchantResult>> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
