using MediatR;
using System.Collections.Generic;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetPaginatedJournalsQuery : IRequest<(IList<BPT.FMS.Domain.Entities.Journal>, int, int)>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = "Type asc";
        public string? search { get; set; }
    }
}

