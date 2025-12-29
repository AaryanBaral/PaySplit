using Microsoft.AspNetCore.Mvc;

using PaySplit.API.Dto.Common;
using PaySplit.API.Dto.Merchants;
using PaySplit.Application.Common.Filter;
using MediatR;
using PaySplit.Application.Merchants.Command.ActivateMerchant;
using PaySplit.Application.Merchants.Command.CreateMerchant;
using PaySplit.Application.Merchants.Command.DeactivateMerchant;
using PaySplit.Application.Merchants.Command.SuspendMerchant;
using PaySplit.Application.Merchants.Command.UpdateMerchant;
using PaySplit.Application.Merchants.Query.GetAllMerchant;
using PaySplit.Application.Merchants.Query.GetMerchantById;

namespace PaySplit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] CreateMerchantRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateMerchantCommand(
                request.TenantId,
                request.Name,
                request.Email,
                request.RevenueSharePercentage);

            var result = await _mediator.Send(command, cancellationToken);

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
            var result = await _mediator.Send(query, cancellationToken);

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
            var result = await _mediator.Send(query, cancellationToken);

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

            var result = await _mediator.Send(command, cancellationToken);

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
            var result = await _mediator.Send(command, cancellationToken);

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
            var result = await _mediator.Send(command, cancellationToken);

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
            var result = await _mediator.Send(command, cancellationToken);

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
