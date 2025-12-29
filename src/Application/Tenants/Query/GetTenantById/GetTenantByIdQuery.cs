
using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdQuery : IRequest<Result<GetTenantByIdDto>>
    {
        public Guid Id { get; }

        public GetTenantByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
