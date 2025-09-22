using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        public ApplicationUnitOfWork(ApplicationDbContext applicationDbContext,
            IChartOfAccountRepository chartOfAccountRepository,
            IVoucherRepository voucherRepository,
            ApplicationDbContext context) : base(applicationDbContext)
        {
            ChartOfAccountRepository = chartOfAccountRepository;
            VoucherRepository = voucherRepository;
            _context = context;
        }

        public async Task<IEnumerable<VoucherEntry>> GetAllByParentIdAsync(Guid parentId)
        {
            try
            {
                return await _context.VoucherEntries
                                .AsNoTracking()
                                .Where(v => v.VoucherId == parentId)
                                .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving voucher entries" + ex.Message);
                throw;
            }
        }
    }
}
