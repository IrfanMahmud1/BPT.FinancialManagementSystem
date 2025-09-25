using BPT.FMS.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalDeleteCommandHandler : IRequestHandler<JournalDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public JournalDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(JournalDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.JournalRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}

