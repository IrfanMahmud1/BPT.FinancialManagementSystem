using BPT.FMS.Domain;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IChartOfAccountRepository ChartOfAccountRepository { get; private set; }
        public IVoucherRepository VoucherRepository { get; private set; }
        public ApplicationUnitOfWork(ApplicationDbContext applicationDbContext,
            IChartOfAccountRepository chartOfAccountRepository,
            IVoucherRepository voucherRepository) : base(applicationDbContext)
        {
            ChartOfAccountRepository = chartOfAccountRepository;
            VoucherRepository = voucherRepository;
        }
    }
}
