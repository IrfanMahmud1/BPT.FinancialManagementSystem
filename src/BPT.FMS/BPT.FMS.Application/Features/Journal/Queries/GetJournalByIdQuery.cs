using MediatR;
using System;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetJournalByIdQuery : IRequest<BPT.FMS.Domain.Entities.Journal?>
    {
        public Guid Id { get; set; }
    }
}

