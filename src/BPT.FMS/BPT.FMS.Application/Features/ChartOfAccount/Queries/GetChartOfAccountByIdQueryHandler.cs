using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Queries
{
    public class GetChartOfAccountByIdQueryHandler : IRequestHandler<GetChartOfAccountByIdQuery, BPT.FMS.Domain.Entities.ChartOfAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetChartOfAccountByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<BPT.FMS.Domain.Entities.ChartOfAccount> Handle(GetChartOfAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ChartOfAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
