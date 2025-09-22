using BPT.FMS.Application.Features.Voucher.Queries;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetAllVoucherEntriesByParentIdQueryHandler : IRequestHandler<GetAllVoucherEntriesByParentIdQuery, IEnumerable<VoucherEntryDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetAllVoucherEntriesByParentIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IEnumerable<VoucherEntryDto>> Handle(GetAllVoucherEntriesByParentIdQuery request, CancellationToken cancellationToken)
        {
            var entries = await _applicationUnitOfWork.GetAllByParentIdAsync(request.ParentId);
            return entries.Select(entry => new VoucherEntryDto
            {
                Id = entry.Id,
                VoucherId = entry.VoucherId,
                AccountName = entry.AccountName,
                Debit = entry.Debit,
                Credit = entry.Credit,
            });
        }
    }
}
