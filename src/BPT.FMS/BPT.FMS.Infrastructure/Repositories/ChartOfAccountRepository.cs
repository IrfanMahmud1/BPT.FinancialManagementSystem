using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
    }
}
