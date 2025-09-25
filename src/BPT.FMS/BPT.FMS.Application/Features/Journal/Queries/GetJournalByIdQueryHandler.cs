using BPT.FMS.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetJournalByIdQueryHandler : IRequestHandler<GetJournalByIdQuery, BPT.FMS.Domain.Entities.Journal?>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetJournalByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<BPT.FMS.Domain.Entities.Journal?> Handle(GetJournalByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.JournalRepository.GetByIdAsync(request.Id);
        }
    }
}

