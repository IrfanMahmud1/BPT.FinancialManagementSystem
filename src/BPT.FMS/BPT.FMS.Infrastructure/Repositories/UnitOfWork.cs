using BPT.FMS.Domain;
using BPT.FMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
