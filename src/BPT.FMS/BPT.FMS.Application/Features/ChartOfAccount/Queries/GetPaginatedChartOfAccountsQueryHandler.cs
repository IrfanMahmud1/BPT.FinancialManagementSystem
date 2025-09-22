using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Queries
{
    public class GetPaginatedChartOfAccountsQueryHandler : IRequestHandler<GetPaginatedChartOfAccountsQuery, (IList<BPT.FMS.Domain.Entities.ChartOfAccount>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetPaginatedChartOfAccountsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<BPT.FMS.Domain.Entities.ChartOfAccount>, int, int)> Handle(GetPaginatedChartOfAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ChartOfAccountRepository.GetPagedAccountsAsync(request.pageIndex, request.pageSize, request.sortColumn, request.search);
        }
    }
}
