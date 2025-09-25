using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Entities
{
    public class JournalEntry : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid JournalId { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public Journal? Journal { get; set; } 
        public ChartOfAccount? ChartOfAccount { get; set; }
    }

}
