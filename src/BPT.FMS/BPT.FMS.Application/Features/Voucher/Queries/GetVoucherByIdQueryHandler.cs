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
    public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, BPT.FMS.Domain.Entities.Voucher>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public GetVoucherByIdQueryHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BPT.FMS.Domain.Entities.Voucher> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.VoucherRepository.GetByIdAsync(request.Id);
        }
    }
}
