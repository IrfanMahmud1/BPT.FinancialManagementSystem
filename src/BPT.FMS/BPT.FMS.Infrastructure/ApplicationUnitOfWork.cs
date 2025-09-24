using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using System.Linq.Dynamic.Core;
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
        public async Task<(List<VoucherEntry> data, int total, int totalDisplay)> GetPagedVoucherEntriesAsync(
           Guid voucherId, int pageIndex = 1, int pageSize = 10, string order = "Type asc", string? search = "")
        {
            int total = await _context.Vouchers.CountAsync();

            try
            {
                IQueryable<VoucherEntry> query = _context.VoucherEntries
                                                    .AsNoTracking()
                                                    .AsSplitQuery()
                                                    .Include(v => v.Voucher)
                                                    .Include(v => v.ChartOfAccount);

                query = query.Where(v => v.VoucherId == voucherId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.Credit.ToString().Contains(search) ||
                        c.Debit.ToString().Contains(search) ||
                        c.ChartOfAccount.AccountName.Contains(search) ||
                        c.Voucher.Type.Contains(search)
                    );
                }


                int totalDisplay = await query.CountAsync();

                query = query.OrderBy(order ?? "VoucherType asc"); // requires System.Linq.Dynamic.Core
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving voucher entries ");
                throw;
            }
        }
    }
}
