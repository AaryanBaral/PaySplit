using API.Dto.Common;
using API.Dto.Merchants;
using Application.Common.Filter;
using Application.Merchants.Command.ActivateMerchant;
using Application.Merchants.Command.CreateMerchant;
using Application.Merchants.Command.DeactivateMerchant;
using Application.Merchants.Command.SuspendMerchant;
using Application.Merchants.Command.UpdateMerchant;
using Application.Merchants.Query.GetAllMerchant;
using Application.Merchants.Query.GetMerchantById;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantsController : ControllerBase
    {
        private readonly CreateMerchantHandler _createHandler;
        private readonly UpdateMerchantHandler _updateHandler;
        private readonly ActivateMerchantHandler _activateHandler;
        private readonly DeactivateMerchantHandler _deactivateHandler;
        private readonly SuspendMerchantHandler _suspendHandler;
        private readonly GetAllMerchantHandler _getAllHandler;
        private readonly GetMerchantByIdHandler _getByIdHandler;

        public MerchantsController(
            CreateMerchantHandler createHandler,
            UpdateMerchantHandler updateHandler,
            ActivateMerchantHandler activateHandler,
            DeactivateMerchantHandler deactivateHandler,
            SuspendMerchantHandler suspendHandler,
            GetAllMerchantHandler getAllHandler,
            GetMerchantByIdHandler getByIdHandler)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _activateHandler = activateHandler;
            _deactivateHandler = deactivateHandler;
            _suspendHandler = suspendHandler;
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] CreateMerchantRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateMerchantCommand(
                request.TenantId,
                request.Name,
                request.Email,
                request.RevenueSharePercentage);

            var result = await _createHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.MerchantId,
                Status = result.Value.Status
            };

            return CreatedAtAction(nameof(GetMerchantById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMerchants(
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

            var query = new GetAllMerchantQuery(filter);
            var result = await _getAllHandler.HandleAsync(query, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = result.Value!.Select(m => new MerchantResponse
            {
                Id = m.Id,
                TenantId = m.TenantId,
                Name = m.Name,
                Email = m.Email,
                RevenueShare = m.RevenueShare,
                Status = m.Status,
                CreatedAtUtc = m.CreatedAtUtc
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMerchantById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetMerchantByIdQuery(id);
            var result = await _getByIdHandler.HandleAsync(query, cancellationToken);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            var merchant = result.Value!;

            var response = new MerchantResponse
            {
                Id = merchant.Id,
                TenantId = merchant.TenantId,
                Name = merchant.Name,
                Email = merchant.Email,
                RevenueShare = merchant.RevenueShare,
                Status = merchant.Status,
                CreatedAtUtc = merchant.CreatedAtUtc,
                DeactivatedAtUtc = merchant.DeactivatedAtUtc,
                SuspendedAtUtc = merchant.SuspendedAtUtc
            };

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMerchant(Guid id, [FromBody] UpdateMerchantRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateMerchantCommand(
                id,
                request.Name,
                request.Email,
                request.RevenueSharePercentage);

            var result = await _updateHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var value = result.Value!;
            var response = new MerchantResponse
            {
                Id = value.MerchantId,
                Name = value.Name,
                Email = value.Email,
                RevenueShare = value.RevenueSharePercentage,
                Status = value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> ActivateMerchant(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateMerchantCommand(id);
            var result = await _activateHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.MerchantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> DeactivateMerchant(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateMerchantCommand(id);
            var result = await _deactivateHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.MerchantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }

        [HttpPost("{id:guid}/suspend")]
        public async Task<IActionResult> SuspendMerchant(Guid id, CancellationToken cancellationToken)
        {
            var command = new SuspendMerchantCommand(id);
            var result = await _suspendHandler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var response = new StatusResponse
            {
                Id = result.Value!.MerchantId,
                Status = result.Value.Status
            };

            return Ok(response);
        }
    }
}
