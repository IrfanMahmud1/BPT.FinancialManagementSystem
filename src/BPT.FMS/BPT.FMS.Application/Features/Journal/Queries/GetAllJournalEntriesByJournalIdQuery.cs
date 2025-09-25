using BPT.FMS.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace BPT.FMS.Application.Features.Journal.Queries
{
    public class GetAllJournalEntriesByJournalIdQuery : IRequest<List<JournalEntryDto>>
    {
        public Guid JournalId { get; set; }
    }
}

