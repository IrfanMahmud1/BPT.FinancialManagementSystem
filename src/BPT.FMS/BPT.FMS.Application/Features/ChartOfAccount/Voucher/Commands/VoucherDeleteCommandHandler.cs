using BPT.FMS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Voucher.Commands
{
    public class VoucherDeleteCommandHandler : IRequestHandler<VoucherDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public VoucherDeleteCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(VoucherDeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.VoucherRepository.RemoveAsync(request.Id);
        }
    }
}
