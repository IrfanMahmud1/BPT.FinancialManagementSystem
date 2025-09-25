using BPT.FMS.Application.Features.Journal.Commands;
using BPT.FMS.Application.Features.Journal.Queries;
using BPT.FMS.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BPT.FMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JournalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("entries")]
        public async Task<ActionResult<List<JournalEntryDto>>> GetAllJournalEntries([FromQuery] Guid journalId)
        {
            try
            {
                var entries = await _mediator.Send(new GetAllJournalEntriesByJournalIdQuery { JournalId = journalId });
                return entries;
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving journal entries.");
            }
        }

        [HttpGet("entries/paginated")]
        public async Task<ActionResult> GetJournalEntriesPaginated([FromQuery] Guid journalId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortColumn = "Debit asc", [FromQuery] string? search = "")
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(new GetPaginatedJournalEntriesQuery
                {
                    journalId = journalId,
                    pageIndex = pageIndex,
                    pageSize = pageSize,
                    sortColumn = sortColumn ?? "Debit asc",
                    search = search
                });

                var entries = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = data.Select(e => new string[]
                    {
                        HttpUtility.HtmlEncode(e.ChartOfAccount?.AccountName ?? e.ChartOfAccountId.ToString()),
                        e.Debit.ToString("0.00"),
                        e.Credit.ToString("0.00"),
                        e.Id.ToString()
                    }).ToArray()
                };

                return Ok(entries);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving journal entries.");
            }
        }

        [HttpGet("paginated")]
        public async Task<ActionResult> GetJournals([FromQuery] GetPaginatedJournalsQuery query)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(query);

                var journals = new
                {
                    recordsTotal = total,
                    recordsFiltered = totalDisplay,
                    data = data.Select(v => new string[]
                    {
                        HttpUtility.HtmlEncode(v.Type),
                        HttpUtility.HtmlEncode(v.ReferenceNo),
                        v.Date.ToString("yyyy-MM-dd"),
                        v.Id.ToString()
                    }).ToArray()
                };

                return Ok(journals);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving journals.");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JournalDto>> GetJournal(Guid id)
        {
            try
            {
                var journal = await _mediator.Send(new GetJournalByIdQuery { Id = id });
                if (journal == null) return NotFound();

                var response = new JournalDto
                {
                    Id = journal.Id,
                    Type = journal.Type,
                    Date = journal.Date,
                    ReferenceNo = journal.ReferenceNo
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving journal.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostJournal(JournalDto model)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"API: Received journal creation request - Type: {model.Type}, ReferenceNo: {model.ReferenceNo}, Entries: {model.Entries?.Count ?? 0}");
                
                var id = Guid.NewGuid();

                await _mediator.Send(new JournalAddCommand
                {
                    Id = id,
                    Type = model.Type,
                    Date = model.Date,
                    ReferenceNo = model.ReferenceNo,
                    Entries = model.Entries.Select(e => new BPT.FMS.Domain.Entities.JournalEntry
                    {
                        Id = Guid.NewGuid(),
                        ChartOfAccountId = e.ChartOfAccountId,
                        Debit = e.Debit,
                        Credit = e.Credit,
                        JournalId = id
                    }).ToList()
                });

                return CreatedAtAction(nameof(GetJournal), new { id }, model);
            }
            catch (DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating a new journal.");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> PutJournal(Guid id, [FromBody] JournalDto model)
        {
            if (id != model.Id) return BadRequest("Mismatched journal ID");
            try
            {
                await _mediator.Send(new JournalUpdateCommand
                {
                    Id = model.Id,
                    Type = model.Type,
                    Date = model.Date,
                    ReferenceNo = model.ReferenceNo
                });
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating journal.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteJournal(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid journal ID");

            try
            {
                var journal = await _mediator.Send(new GetJournalByIdQuery { Id = id });
                if (journal == null) return NotFound();

                await _mediator.Send(new JournalDeleteCommand { Id = id });

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting journal.");
            }
        }
    }
}

