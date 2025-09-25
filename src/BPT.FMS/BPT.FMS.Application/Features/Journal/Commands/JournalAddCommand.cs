using MediatR;
using System;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalAddCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string Type { get; set; }
        public List<BPT.FMS.Domain.Entities.JournalEntry> Entries { get; set; } = new();
    }
}

