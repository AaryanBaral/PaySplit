using Application.Common.Abstractions;
using Application.Common.Results;
using Application.Interfaces.Presistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Tenants.Query.GetAllTenant
{
    public class GetAllTenantHandler : IQueryHandler<GetAllTenantQuery, Result<List<GetAllTenantDto>>>
    {
        private readonly IApplicationDbContext _db;
        public GetAllTenantHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Result<List<GetAllTenantDto>>> HandleAsync(GetAllTenantQuery query, CancellationToken cancellationToken = default)
        {
            var filter = query.PaginationFilter;

            var dbQuery = _db.Tenants
                .AsNoTracking()          // no tracking → lighter memory usage
                .AsQueryable();

            //  Search by name (case-insensitive), only if search is provided
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLower();

                dbQuery = dbQuery.Where(t =>
                    t.Name.ToLower().Contains(search));
            }

            //  Filter by status, only if provided
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                dbQuery = dbQuery.Where(t =>
                    t.Status.ToString() == filter.Status);
            }

            // Paging 
            var skip = (filter.Page - 1) * filter.PageSize;

            var tenants = await dbQuery
                .OrderBy(t => t.CreatedAtUtc)
                .Skip(skip)
                .Take(filter.PageSize)
                .Select(t => new GetAllTenantDto(
                    t.Id,
                    t.Name,
                    t.Status.ToString(),
                    t.CreatedAtUtc))
                .ToListAsync(cancellationToken);

            return Result<List<GetAllTenantDto>>.Success(tenants);


        }
    }
}