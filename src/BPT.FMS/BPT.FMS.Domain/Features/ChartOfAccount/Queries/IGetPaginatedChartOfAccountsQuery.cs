
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Features.ChartOfAccount.Queries
{
    public interface IGetPaginatedChartOfAccountsQuery
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; }
        public string? sortColumn { get; set; }
    }
}
