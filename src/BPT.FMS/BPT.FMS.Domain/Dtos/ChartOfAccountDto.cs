using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Dtos
{
    public class ChartOfAccountDto
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
