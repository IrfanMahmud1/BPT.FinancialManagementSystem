using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.ChartOfAccount.Commands
{
    public class ChartOfAccountDeleteCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
