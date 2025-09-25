using BPT.FMS.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetPaginatedJournalEntriesQueryHandler : IRequestHandler<GetPaginatedJournalEntriesQuery, (IList<BPT.FMS.Domain.Entities.JournalEntry>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetPaginatedJournalEntriesQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<BPT.FMS.Domain.Entities.JournalEntry>, int, int)> Handle(GetPaginatedJournalEntriesQuery request, CancellationToken cancellationToken)
        {
            var (data, total, totalDisplay) = await _applicationUnitOfWork.GetPagedJournalEntriesAsync(request.journalId, request.pageIndex, request.pageSize, request.sortColumn, request.search);
            return (data, total, totalDisplay);
        }
    }
}

