using System;
using System.Collections.Generic;

namespace BPT.FMS.Domain.Dtos
{
    public class JournalDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Type { get; set; }
        public List<JournalEntryDto> Entries { get; set; } = new();
    }
}

