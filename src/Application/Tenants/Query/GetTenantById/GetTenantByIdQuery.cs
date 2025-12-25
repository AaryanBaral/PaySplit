
using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdQuery:IQuery<Result<GetTenantByIdDto>>
    {
        public Guid Id {get;}

        public GetTenantByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}