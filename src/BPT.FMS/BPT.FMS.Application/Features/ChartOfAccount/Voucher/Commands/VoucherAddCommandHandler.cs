using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Commands
{
    public class VoucherAddCommandHandler : IRequestHandler<VoucherAddCommand>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public VoucherAddCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(VoucherAddCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.VoucherRepository.CreateVoucherWithEntriesAsync(new BPT.FMS.Domain.Entities.Voucher
            {
                Id = request.Id,
                Date = request.Date,
                ReferenceNo = request.ReferenceNo,
                Type = request.Type,
                Entries = request.Entries
            });
        }
    }
}
