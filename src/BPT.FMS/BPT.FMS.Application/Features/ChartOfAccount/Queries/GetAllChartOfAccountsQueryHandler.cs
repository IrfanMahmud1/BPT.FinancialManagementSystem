using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Queries
{
    public class GetAllChartOfAccountsQueryHandler : IRequestHandler<GetAllChartOfAccountsQuery, IEnumerable<BPT.FMS.Domain.Entities.ChartOfAccount>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetAllChartOfAccountsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IEnumerable<BPT.FMS.Domain.Entities.ChartOfAccount>> Handle(GetAllChartOfAccountsQuery request, CancellationToken cancellationToken)
        {
           return await _applicationUnitOfWork.ChartOfAccountRepository.GetAllAsync();
        }
    }
}
