using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Features.Voucher.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Commands
{
    public class VoucherAddCommand : IRequest, IVoucherAddCommand
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Type { get; set; }
        public List<VoucherEntry> Entries { get; set; } = new();
    }
}
