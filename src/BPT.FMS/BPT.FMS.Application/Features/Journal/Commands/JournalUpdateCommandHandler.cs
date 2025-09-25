using BPT.FMS.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalUpdateCommandHandler : IRequestHandler<JournalUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public JournalUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(JournalUpdateCommand request, CancellationToken cancellationToken)
        {
            var journal = await _applicationUnitOfWork.JournalRepository.GetByIdAsync(request.Id);
            if (journal == null) return;
            journal.Type = request.Type;
            journal.Date = request.Date;
            journal.ReferenceNo = request.ReferenceNo;

            await _applicationUnitOfWork.JournalRepository.EditAsync(journal);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}

