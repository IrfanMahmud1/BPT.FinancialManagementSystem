using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Queries
{
    public class GetChartOfAccountByIdQuery : IRequest<BPT.FMS.Domain.Entities.ChartOfAccount>
    {
        public Guid Id { get; set; }
    }
}
