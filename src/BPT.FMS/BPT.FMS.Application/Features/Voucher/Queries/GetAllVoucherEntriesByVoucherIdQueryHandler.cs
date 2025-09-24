using BPT.FMS.Application.Features.Voucher.Queries;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetAllVoucherEntriesByVoucherIdQueryHandler : IRequestHandler<GetAllVoucherEntriesByVoucherIdQuery, List<BPT.FMS.Domain.Dtos.VoucherEntryDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetAllVoucherEntriesByVoucherIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<List<BPT.FMS.Domain.Dtos.VoucherEntryDto>> Handle(GetAllVoucherEntriesByVoucherIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.GetVoucherEntriesByVoucherIdAsync(request.voucherId);
        }
    }
}
