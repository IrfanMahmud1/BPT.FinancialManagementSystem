using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetAllVoucherEntriesByVoucherIdQuery :  IRequest<List<VoucherEntryDto>> ,IGetAllVoucherEntriesByParentIdQuery
    {
        public Guid voucherId { get; set; }
    }
}
