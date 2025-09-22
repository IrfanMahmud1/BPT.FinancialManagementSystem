using BPT.FMS.Application.Exceptions;
using BPT.FMS.Application.Features.ChartOfAccount.Commands;
using BPT.FMS.Application.Features.ChartOfAccount.Queries;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BPT.FMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartOfAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartOfAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("all/{childId:guid}")]
        public async Task<ActionResult<IEnumerable<ChartOfAccountDto>>> GetAllChartOfAccounts(Guid childId)
        {
            try
            {
                var accounts = await _mediator.Send(new GetAllChartOfAccountsQuery{ Id = childId });
                var dtos = accounts.Select(acc => new ChartOfAccountDto
                {
                    Id = acc.Id,
                    AccountName = acc.AccountName,
                    AccountType = acc.AccountType,
                    ParentId = acc.ParentId,
                    IsActive = acc.IsActive,
                    CreatedAt = acc.CreatedAt
                });
                return Ok(dtos);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving chart of accounts.");
            }
        }

        // GET: api/ChartOfAccount/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult> GetChartOfAccounts([FromQuery] GetPaginatedChartOfAccountsQuery query)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(query);

                var accounts = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = data.Select(acc => new string[]
                    {
                        HttpUtility.HtmlEncode(acc.AccountName),
                        HttpUtility.HtmlEncode(acc.AccountType),
                        HttpUtility.HtmlEncode(acc.Parent != null ? acc.Parent.AccountName : "N/A"),
                        acc.CreatedAt.ToString("yyyy-MM-dd"),
                        acc.Id.ToString()
                    }).ToArray()
                };

                return Ok(accounts);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving chart of accounts.");
            }
        }

        // GET: api/ChartOfAccount/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ChartOfAccountDto>> GetChartOfAccount(Guid id)
        {
            try
            {
                var account = await _mediator.Send(new GetChartOfAccountByIdQuery { Id = id });
                if (account == null) return NotFound();

                var response = new ChartOfAccountDto
                {
                    Id = account.Id,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    ParentId = account.ParentId,
                    IsActive = account.IsActive,
                    CreatedAt = account.CreatedAt
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving chart of account.");
            }
        }

        // POST: api/ChartOfAccount
        [HttpPost]
        public async Task<ActionResult> PostChartOfAccount(VoucherDto dto)
        {

            try
            {
                var id = Guid.NewGuid();

                await _mediator.Send(new ChartOfAccountAddCommand
                {
                    Id = id,
                    AccountName = dto.AccountName,
                    ParentId = dto.ParentId,
                    AccountType = dto.AccountType,
                    IsActive = dto.IsActive,
                    CreatedAt = dto.CreatedAt
                });

                return CreatedAtAction(nameof(GetChartOfAccount), new { id }, dto);
            }
            catch (DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating a new chart of account.");
            }
        }

        // PUT: api/ChartOfAccount/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> PutChartOfAccount(Guid id, [FromBody] ChartOfAccountDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched account ID");

            try
            {
                await _mediator.Send(new ChartOfAccountUpdateCommand
                {
                    Id = dto.Id,
                    AccountName = dto.AccountName,
                    ParentId = dto.ParentId,
                    AccountType = dto.AccountType,
                    IsActive = dto.IsActive
                });

                return NoContent();
            }
            catch (DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating chart of account.");
            }
        }

        // DELETE: api/ChartOfAccount/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteChartOfAccount(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid account ID");

            try
            {
                var account = await _mediator.Send(new GetChartOfAccountByIdQuery { Id = id });
                if (account == null) return NotFound();

                await _mediator.Send(new ChartOfAccountDeleteCommand { Id = id });

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting chart of account.");
            }
        }
    }

}
