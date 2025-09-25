using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalAddCommandHandler : IRequestHandler<JournalAddCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public JournalAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<Guid> Handle(JournalAddCommand request, CancellationToken cancellationToken)
        {
            var journal = new BPT.FMS.Domain.Entities.Journal
            {
                Id = request.Id,
                Date = request.Date,
                ReferenceNo = request.ReferenceNo,
                Type = request.Type,
                Entries = request.Entries
            };

            await _applicationUnitOfWork.JournalRepository.CreateJournalWithEntriesAsync(journal);

            return journal.Id;
        }
    }
}

