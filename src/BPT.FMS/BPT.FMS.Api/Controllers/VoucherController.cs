using BPT.FMS.Application.Exceptions;
using BPT.FMS.Application.Features.Voucher.Commands;
using BPT.FMS.Application.Features.Voucher.Queries;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BPT.FMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VoucherController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("all/{voucherId:guid}")]
        public async Task<ActionResult<IEnumerable<VoucherEntryDto>>> GetAllVoucherEntries(Guid voucherId)
        {
            try
            {
                var entries = await _mediator.Send(new GetAllVoucherEntriesByParentIdQuery{ ParentId = voucherId });
                var dtos = entries.Select(acc => new VoucherEntryDto
                {
                    Id = acc.Id,
                    AccountName = acc.AccountName,
                    VoucherId = acc.VoucherId,
                    Debit = acc.Debit,
                    Credit = acc.Credit,
                });
                return Ok(dtos);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving voucher entries.");
            }
        }

        // GET: api/Voucher/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult> GetVouchers([FromQuery] GetPaginatedVouchersQuery query)
        {
            try
            {
                var (data, total, totalDisplay) = await _mediator.Send(query);

                var vouchers = new
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

                return Ok(vouchers);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving vouchers.");
            }
        }

        // GET: api/Voucher/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VoucherDto>> GetVoucher(Guid id)
        {
            try
            {
                var voucher = await _mediator.Send(new GetVoucherByIdQuery { Id = id });
                if (voucher == null) return NotFound();

                var response = new VoucherDto
                {
                    Id = voucher.Id,
                    Type = voucher.Type,
                    Date = voucher.Date,
                    ReferenceNo = voucher.ReferenceNo
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving voucher.");
            }
        }

        // POST: api/Voucher
        [HttpPost]
        public async Task<ActionResult> PostVoucher(VoucherDto dto)
        {

            try
            {
                var id = Guid.NewGuid();

                await _mediator.Send(new VoucherAddCommand
                {
                    Id = id,
                    Type = dto.Type,
                    Date = dto.Date,
                    ReferenceNo = dto.ReferenceNo,
                    Entries = dto.Entries.Select(e => new VoucherEntry
                    {
                        Id = e.Id,
                        AccountName = e.AccountName,
                        VoucherId = id,
                        Debit = e.Debit,
                        Credit = e.Credit,
                    }).ToList()
                });

                return CreatedAtAction(nameof(GetVoucher), new { id }, dto);
            }
            catch (DuplicateNameException dex)
            {
                return BadRequest(dex.Message);
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating a new voucher.");
            }
        }

        // DELETE: api/Voucher/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteVoucher(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid voucher ID");

            try
            {
                var account = await _mediator.Send(new GetVoucherByIdQuery { Id = id });
                if (account == null) return NotFound();

                await _mediator.Send(new VoucherDeleteCommand { Id = id });

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting voucher.");
            }
        }
    }

}
