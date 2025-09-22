using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure.Repositories
{
    public class ChartOfAccountRepository : Repository<ChartOfAccount, Guid>, IChartOfAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public ChartOfAccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsAccountNameDuplicateAsync(string name, Guid? id)
        {
            return await _context.ChartOfAccounts
                    .AnyAsync(x => x.AccountName == name && (!id.HasValue || x.Id != id.Value));
        }

        public async Task<(List<ChartOfAccount> data, int total, int totalDisplay)> GetPagedAccountsAsync(
    int pageIndex = 1, int pageSize = 10, string order = "AccountName asc", string? search = "")
        {
            int total = await _context.ChartOfAccounts.CountAsync();

            try
            {
                IQueryable<ChartOfAccount> query = _context.ChartOfAccounts
                                                    .AsNoTracking()
                                                    .AsSplitQuery()
                                                    .Include(c => c.Parent);


                if (DateTime.TryParse(search, out var searchDate))
                {
                    query = query.Where(c => c.CreatedAt.Date == searchDate.Date);
                }
                else if(!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.AccountName.Contains(search) ||
                        c.AccountType.Contains(search) 
                    );
                }


                int totalDisplay = await query.CountAsync();

                query = query.OrderBy(order ?? "AccountName asc"); // requires System.Linq.Dynamic.Core
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving chart of accounts ");
                throw;
            }
        }

    }
}
