using BPT.FMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Repositories
{
    public interface IChartOfAccountRepository : IRepository<ChartOfAccount, Guid>
    {
        Task<bool> IsAccountNameDuplicateAsync(string name, Guid? id);
    }
}
