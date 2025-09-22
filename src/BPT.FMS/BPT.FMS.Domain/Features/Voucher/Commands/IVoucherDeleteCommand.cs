using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Voucher.Commands
{
    public interface IVoucherDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
