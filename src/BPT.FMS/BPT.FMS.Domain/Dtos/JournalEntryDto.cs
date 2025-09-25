using System;

namespace BPT.FMS.Domain.Dtos
{
    public class JournalEntryDto
    {
        public Guid Id { get; set; }
        public Guid JournalId { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public JournalDto? Journal { get; set; }
        public ChartOfAccountDto? ChartOfAccount { get; set; }
    }
}

