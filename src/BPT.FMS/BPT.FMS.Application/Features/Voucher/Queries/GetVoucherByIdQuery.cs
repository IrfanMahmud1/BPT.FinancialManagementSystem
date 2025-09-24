using BPT.FMS.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetVoucherByIdQuery : IRequest<BPT.FMS.Domain.Entities.Voucher>
    {
        public Guid Id { get; set; }
    }
}
