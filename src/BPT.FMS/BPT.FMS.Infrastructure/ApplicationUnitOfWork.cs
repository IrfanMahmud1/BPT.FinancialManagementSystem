using BPT.FMS.Domain;
using BPT.FMS.Infrastructure.Data;
using BPT.FMS.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            
        }
    }
}
