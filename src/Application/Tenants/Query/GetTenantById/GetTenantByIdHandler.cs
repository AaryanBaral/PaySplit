
using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Tenants.Query.GetTenantById
{
    public class GetTenantByIdHandler : IQueryHandler<GetTenantByIdQuery, Result<GetTenantByIdDto>>
    {
        private readonly IApplicationDbContext _db;
        public GetTenantByIdHandler(IApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Result<GetTenantByIdDto>> HandleAsync(GetTenantByIdQuery query, CancellationToken cancellationToken = default)
        {
            if (query.Id == Guid.Empty)
            {
                return Result<GetTenantByIdDto>.Failure("Tenant id is required.");
            }
            var tenant = await _db.Tenants
            .AsNoTracking()
            .Where(t => t.Id == query.Id)
            .Select(t => new GetTenantByIdDto(t.Id, t.Name, t.Status.ToString(), t.CreatedAtUtc, t.DeactivatedAtUtc, t.SuspendedAtUTC))
            .FirstOrDefaultAsync(cancellationToken);
            if (tenant is null)
            {
                return Result<GetTenantByIdDto>.Failure("Tenant not found");
            }
            return Result<GetTenantByIdDto>.Success(tenant);

        }
    }
}