using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Commands
{
    public class ChartOfAccountDeleteCommandHandler : IRequestHandler<ChartOfAccountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ChartOfAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task Handle(ChartOfAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _applicationUnitOfWork.ChartOfAccountRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _applicationUnitOfWork.ChartOfAccountRepository.RemoveAsync(entity.Id);
                await _applicationUnitOfWork.SaveAsync();
            }
        }
    }
}
