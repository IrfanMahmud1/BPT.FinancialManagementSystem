using BPT.FMS.Application.Exceptions;
using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Commands
{
    public class ChartOfAccountUpdateCommandHandler : IRequestHandler<ChartOfAccountUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ChartOfAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task Handle(ChartOfAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _applicationUnitOfWork.ChartOfAccountRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                if (!await _applicationUnitOfWork.ChartOfAccountRepository.IsAccountNameDuplicateAsync(request.AccountName, request.Id))
                {
                    entity.AccountName = request.AccountName;
                    entity.ParentId = request.ParentId;
                    entity.AccountType = request.AccountType;
                    entity.IsActive = request.IsActive;
                    await _applicationUnitOfWork.ChartOfAccountRepository.EditAsync(entity);
                    await _applicationUnitOfWork.SaveAsync();
                }
                else
                {
                    throw new DuplicateNameException("Account");
                }
            }
        }
    }
}
