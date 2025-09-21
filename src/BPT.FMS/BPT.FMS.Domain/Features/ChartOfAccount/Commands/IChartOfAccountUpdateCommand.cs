using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Features.ChartOfAccount.Commands
{
    public interface IChartOfAccountUpdateCommand
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid? ParentId { get; set; }
        public string AccountType { get; set; } // Asset, Liability, Income, etc.
        public bool IsActive { get; set; }
    }
}
