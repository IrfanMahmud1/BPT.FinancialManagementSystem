using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Features.ChartOfAccount.Commands
{
    public interface IChartOfAccountDeleteCommand 
    {
        public Guid Id { get; set; }
    }
}
