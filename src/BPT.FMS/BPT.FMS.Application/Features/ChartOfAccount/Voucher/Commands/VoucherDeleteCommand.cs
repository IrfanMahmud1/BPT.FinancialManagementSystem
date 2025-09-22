using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Commands
{
    public class VoucherDeleteCommand : IRequest, IVoucherDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
