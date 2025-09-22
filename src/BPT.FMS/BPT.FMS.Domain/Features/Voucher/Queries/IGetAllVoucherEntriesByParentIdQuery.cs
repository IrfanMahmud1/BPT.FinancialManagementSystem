using BPT.FMS.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.Voucher.Queries
{
    public interface IGetAllVoucherEntriesByParentIdQuery
    {
        public Guid ParentId { get; set; }
    }
}
