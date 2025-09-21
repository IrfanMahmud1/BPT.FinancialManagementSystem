using BPT.FMS.Domain;
using BPT.FMS.Domain.Features.ChartOfAccount.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Queries
{
    public class GetPaginatedChartOfAccountsQuery : DataTables, IRequest<(IList<BPT.FMS.Domain.Entities.ChartOfAccount>, int, int)>,IGetPaginatedChartOfAccountsQuery
    {
    }
}
