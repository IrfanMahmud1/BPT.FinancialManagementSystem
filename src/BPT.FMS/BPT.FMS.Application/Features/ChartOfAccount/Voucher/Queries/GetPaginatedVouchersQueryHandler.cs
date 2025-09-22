using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public class GetPaginatedVouchersQueryHandler : IRequestHandler<GetPaginatedVouchersQuery, (IList<BPT.FMS.Domain.Entities.Voucher>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GetPaginatedVouchersQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<(IList<Domain.Entities.Voucher>, int, int)> Handle(GetPaginatedVouchersQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.VoucherRepository.GetPagedVouchersAsync(request.pageIndex, request.pageSize, request.sortColumn, request.search);
        }
    }
}
