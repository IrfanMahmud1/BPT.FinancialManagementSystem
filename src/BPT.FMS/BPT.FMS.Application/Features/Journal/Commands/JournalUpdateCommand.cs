using MediatR;
using System;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalUpdateCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public bool ReplaceEntries { get; set; } = false;
    }
}

