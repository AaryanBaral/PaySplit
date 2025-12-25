using API.Dto.Common;
using API.Dto.Tenants;
using Application.Common.Filter;
using Application.Common.Results;
using Application.Tenants.Command.ActivateTenant;
using Application.Tenants.Command.CreateTenant;
using Application.Tenants.Command.DeactivateTenant;
using Application.Tenants.Command.SuspendTenant;
using Application.Tenants.Command.UpdateTenant;
using Application.Tenants.Query.GetAllTenant;
using Application.Tenants.Query.GetTenantById;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ILogger<TenantsController> _logger;
        private readonly CreateTenantHandler _createHandler;
        private readonly UpdateCommandHandler _renameHandler;
        private readonly ActivateCommandHandler _activateHandler;
        private readonly DeactivateCommandHandler _deactivateHandler;
        private readonly SuspendCommandHandler _suspendHandler;
        private readonly GetAllTenantHandler _getAllHandler;
        private readonly GetTenantByIdHandler _getByIdHandler;

        public TenantsController(
            ILogger<TenantsController> logger,
            CreateTenantHandler createHandler,
            UpdateCommandHandler renameHandler,
            ActivateCommandHandler activateHandler,
            DeactivateCommandHandler deactivateHandler,
            SuspendCommandHandler suspendHandler,
            GetAllTenantHandler getAllHandler,
            GetTenantByIdHandler getByIdHandler)
        {
            _logger = logger;
            _createHandler = createHandler;
            _renameHandler = renameHandler;
            _activateHandler = activateHandler;
            _deactivateHandler = deactivateHandler;
            _suspendHandler = suspendHandler;
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating tenant with name {TenantName}", request.Name);
            var command = new CreateTenantCommand(request.Name);
            var result = await _createHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Create tenant failed: {Error}", result.Error);
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
            _logger.LogInformation("Fetching tenants page {Page} size {PageSize}", page, pageSize);
            var filter = new PaginationFilter
            {
                Search = search,
                Status = status,
                Page = page,
                PageSize = pageSize
            };

            var query = new GetAllTenantQuery(filter);
            var result = await _getAllHandler.HandleAsync(query, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Get tenants failed: {Error}", result.Error);
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
            _logger.LogInformation("Fetching tenant {TenantId}", id);
            var query = new GetTenantByIdQuery(id);
            var result = await _getByIdHandler.HandleAsync(query, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Tenant {TenantId} not found: {Error}", id, result.Error);
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
            _logger.LogInformation("Renaming tenant {TenantId}", id);
            var command = new RenameTenantCommand(id, request.Name);
            var result = await _renameHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Rename tenant {TenantId} failed: {Error}", id, result.Error);
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
            _logger.LogInformation("Activating tenant {TenantId}", id);
            var command = new ActivateTenantCommand(id);
            var result = await _activateHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Activate tenant {TenantId} failed: {Error}", id, result.Error);
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
            _logger.LogInformation("Deactivating tenant {TenantId}", id);
            var command = new DeactivateTenantCommand(id);
            var result = await _deactivateHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Deactivate tenant {TenantId} failed: {Error}", id, result.Error);
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
            _logger.LogInformation("Suspending tenant {TenantId}", id);
            var command = new SuspendTenantCommand(id);
            var result = await _suspendHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Suspend tenant {TenantId} failed: {Error}", id, result.Error);
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
