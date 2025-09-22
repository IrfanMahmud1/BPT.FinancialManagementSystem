using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public interface IGetPaginatedVouchersQuery
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; }
        public string? sortColumn { get; set; }
    }
}
