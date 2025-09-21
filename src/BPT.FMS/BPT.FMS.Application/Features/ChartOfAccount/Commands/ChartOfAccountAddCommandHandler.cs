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
    public class ChartOfAccountAddCommandHandler : IRequestHandler<ChartOfAccountAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ChartOfAccountAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task Handle(ChartOfAccountAddCommand request, CancellationToken cancellationToken)
        {
            var entity = new BPT.FMS.Domain.Entities.ChartOfAccount
            {
                Id = request.Id,
                AccountName = request.AccountName,
                ParentId = request.ParentId,
                AccountType = request.AccountType,
                IsActive = request.IsActive,
                CreatedAt = request.CreatedAt
            };
            if(!await _applicationUnitOfWork.ChartOfAccountRepository.IsAccountNameDuplicateAsync(request.AccountName, null))
            {
                await _applicationUnitOfWork.ChartOfAccountRepository.AddAsync(entity);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
            {
                throw new DuplicateNameException("Account");
            }
        }
    }
}
