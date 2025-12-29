using PaySplit.Application.Common.Mappings;
using PaySplit.Application.Common.Results;
using PaySplit.Application.Interfaces.Repository;
using MediatR;

namespace PaySplit.Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantHandler: IRequestHandler<GetAllMerchantQuery, Result<List<GetAllMerchantDto>>>
    {
        private readonly IMerchantRepository _repository;

        public GetAllMerchantHandler(IMerchantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetAllMerchantDto>>> HandleAsync(GetAllMerchantQuery query, CancellationToken cancellationToken = default)
        {
            var filter = query.PaginationFilter;

            var merchants = await _repository.GetAllAsync(filter, cancellationToken);

            var result = merchants
                .Select(m => m.ToGetAllMerchantDto())
                .ToList();

            return Result<List<GetAllMerchantDto>>.Success(result);
        }
    
        public Task<Result<List<GetAllMerchantDto>>> Handle(GetAllMerchantQuery request, CancellationToken cancellationToken)
            => HandleAsync(request, cancellationToken);
}
}
