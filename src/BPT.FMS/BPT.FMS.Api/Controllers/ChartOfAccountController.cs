using BPT.FMS.Application.Exceptions;
using BPT.FMS.Application.Features.ChartOfAccount.Commands;
using BPT.FMS.Application.Features.ChartOfAccount.Queries;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Web;

namespace BPT.FMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartOfAccountController : ControllerBase
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public ChartOfAccountController(IMediator mediator,
            IApplicationUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        // GET: api/ChartOfAccounts
        [HttpPost]
        public async Task<ActionResult> GetChartOfAccounts(GetPaginatedChartOfAccountsQuery query)
        {
            try
            {
                var (data,total,totalDisplay) = await _mediator.Send(query);
                var accounts = (from acc in data
                                select new string[]
                                {
                                    HttpUtility.HtmlEncode(acc.AccountName),
                                    HttpUtility.HtmlEncode(acc.AccountType),
                                    HttpUtility.HtmlEncode(acc.Parent != null ? acc.Parent.AccountName : ""),
                                    acc.CreatedAt.ToString("yyyy-MM-dd"),
                                    acc.Id.ToString()
                                }).ToArray();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving chart of accounts");
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ChartOfAccount>> GetChartOfAccount(Guid id)
        {
            try
            {
                var account = await _mediator.Send(new GetChartOfAccountByIdQuery { Id = id });
                if (account == null) return NotFound();
                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving chart of account");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/ChartOfAccounts
        [HttpPost]
        public async Task<ActionResult> PostChartOfAccount(ChartOfAccount account)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                 await _mediator.Send(
                    new ChartOfAccountAddCommand
                    {
                        Id = Guid.NewGuid(),
                        AccountName = account.AccountName,
                        ParentId = account.ParentId,
                        AccountType = account.AccountType,
                        IsActive = account.IsActive,
                        CreatedAt = DateTime.UtcNow
                    });
            }
            catch(DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a new chart of account");
                return StatusCode(500, "Internal server error");
            }
            return NoContent();
        }

        // PUT: api/ChartOfAccounts/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutChartOfAccount(ChartOfAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (account.Id == Guid.Empty) return BadRequest("Invalid account ID");
            try
            {
                await _mediator.Send(
                    new ChartOfAccountUpdateCommand
                    {
                        Id = account.Id,
                        AccountName = account.AccountName,
                        ParentId = account.ParentId,
                        AccountType = account.AccountType,
                        IsActive = account.IsActive

                    });
            }
            catch (DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating chart of account");
                return StatusCode(500, "Internal server error");
            }
            return NoContent();
        }

        // DELETE: api/ChartOfAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChartOfAccount(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid account ID");
            try
            {
                var account = await _mediator.Send(new GetChartOfAccountByIdQuery { Id = id });
                if (account == null) return NotFound();
                await _mediator.Send(new ChartOfAccountDeleteCommand { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting chart of account");
                return StatusCode(500, "Internal server error");
            }
            return NoContent();
        }
    }
}
