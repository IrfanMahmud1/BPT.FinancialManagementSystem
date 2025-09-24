using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Dtos
{
    public class VoucherEntryDto
    {
        public Guid Id { get; set; }
        public Guid VoucherId { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
