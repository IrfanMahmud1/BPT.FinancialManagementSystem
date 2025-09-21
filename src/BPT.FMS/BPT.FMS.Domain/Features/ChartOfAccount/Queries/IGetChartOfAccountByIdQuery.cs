
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.d.Features.ChartOfAccount.Queries
{
    public interface IGetChartOfAccountByIdQuery    
    {
        public Guid Id { get; set; }
    }
}
