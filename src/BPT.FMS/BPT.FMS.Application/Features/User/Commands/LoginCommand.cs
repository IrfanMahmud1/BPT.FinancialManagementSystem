using BPT.FMS.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.User.Commands
{
    public class LoginCommand : IRequest<AuthenticationResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
