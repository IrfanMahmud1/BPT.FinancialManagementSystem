using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetAllJournalEntriesByJournalIdQueryHandler : IRequestHandler<GetAllJournalEntriesByJournalIdQuery, List<JournalEntryDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetAllJournalEntriesByJournalIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<List<JournalEntryDto>> Handle(GetAllJournalEntriesByJournalIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.GetJournalEntriesByJournalIdAsync(request.JournalId);
        }
    }
}

