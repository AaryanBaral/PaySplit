
using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdQuery : IQuery<Result<GetTenantByIdDto>>
    {
        public Guid Id { get; }

        public GetTenantByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
