using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetPaginatedVouchersQuery : IRequest<(IList<BPT.FMS.Domain.Entities.Voucher>, int, int)>, IGetPaginatedVouchersQuery
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; }
        public string? sortColumn { get; set; }
    }
}
