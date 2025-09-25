using BPT.FMS.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetPaginatedJournalsQueryHandler : IRequestHandler<GetPaginatedJournalsQuery, (IList<BPT.FMS.Domain.Entities.Journal>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetPaginatedJournalsQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<BPT.FMS.Domain.Entities.Journal>, int, int)> Handle(GetPaginatedJournalsQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.JournalRepository.GetPagedJournalsAsync(request.pageIndex, request.pageSize, request.sortColumn, request.search);
        }
    }
}

