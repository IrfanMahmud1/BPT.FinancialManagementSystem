using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Entities
{
    public class ChartOfAccount : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public Guid? ParentId { get; set; }
        public string AccountType { get; set; } // Asset, Liability, Income, etc.
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public List<ChartOfAccount> Children { get; set; } = new();
    }

}
