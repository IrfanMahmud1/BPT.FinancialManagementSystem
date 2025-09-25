using MediatR;
using System;

namespace BPT.FMS.Application.Features.Journal.Commands
{
    public class JournalDeleteCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}

