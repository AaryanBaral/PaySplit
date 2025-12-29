using Microsoft.AspNetCore.Mvc;

using PaySplit.API.Dto.Common;
using PaySplit.API.Dto.Tenants;
using PaySplit.Application.Common.Filter;
using PaySplit.Application.Common.Results;
using MediatR;
using PaySplit.Application.Tenants.Command.ActivateTenant;
using PaySplit.Application.Tenants.Command.CreateTenant;
using PaySplit.Application.Tenants.Command.DeactivateTenant;
using PaySplit.Application.Tenants.Command.SuspendTenant;
using PaySplit.Application.Tenants.Command.UpdateTenant;
using PaySplit.Application.Tenants.Query.GetAllTenant;
using PaySplit.Application.Tenants.Query.GetTenantById;

namespace PaySplit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTenantCommand(request.Name, request.DefaultCurrency);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.TenantId,
                Status = result.Value.Status
            };

            return CreatedAtAction(nameof(GetTenantById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var filter = new PaginationFilter
            {
                Search = search,
                Status = status,
                Page = page,
                PageSize = pageSize
            };

            var query = new GetAllTenantQuery(filter);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = result.Value!.Select(t => new TenantResponse
            {
                Id = t.Id,
                Name = t.Name,
                Status = t.Status,
                CreatedAtUtc = t.CreatedAtUtc
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTenantById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTenantByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            var tenant = result.Value!;

            var response = new TenantResponse
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Status = tenant.Status,
                CreatedAtUtc = tenant.CreatedAtUtc,
                DeactivatedAtUtc = tenant.DeactivatedAtUtc,
                SuspendedAtUtc = tenant.SuspendedAtUtc
            };

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> RenameTenant(Guid id, [FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
        {
            var command = new RenameTenantCommand(id, request.Name);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new TenantResponse
            {
                Id = result.Value!.TenantId,
                Name = result.Value.Name,
                Status = result.Value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> ActivateTenant(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateTenantCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.TenantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> DeactivateTenant(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateTenantCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.TenantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/suspend")]
        public async Task<IActionResult> SuspendTenant(Guid id, CancellationToken cancellationToken)
        {
            var command = new SuspendTenantCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.TenantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }
    }
}
