using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Dtos
{
    public class GetVoucherEntriesDto : DataTables
    {
        public Guid voucherId { get; set; }
    }
}
