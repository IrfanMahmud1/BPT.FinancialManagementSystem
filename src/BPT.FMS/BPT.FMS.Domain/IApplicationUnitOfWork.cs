using BPT.FMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IChartOfAccountRepository ChartOfAccountRepository { get; }
    }
}
