using MediatR;
using System.Collections.Generic;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetPaginatedJournalEntriesQuery : IRequest<(IList<BPT.FMS.Domain.Entities.JournalEntry>, int, int)>
    {
        public System.Guid journalId { get; set; }
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = "Debit asc";
        public string? search { get; set; }
    }
}

