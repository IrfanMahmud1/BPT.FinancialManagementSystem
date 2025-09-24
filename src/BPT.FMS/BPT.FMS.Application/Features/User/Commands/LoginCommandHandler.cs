using BPT.FMS.Applicatiion.Interfaces;
using BPT.FMS.Domain;
using BPT.FMS.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Features.User.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResultDto>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<AuthenticationResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _applicationUnitOfWork.UserRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthenticationResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var isValidPassword = await _applicationUnitOfWork.UserRepository.ValidateCredentialsAsync(request.Email, request.Password);

            if (!isValidPassword)
            {
                return new AuthenticationResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResultDto
            {
                IsSuccess = true,
                Token = token,
                User = user,
                Message = "Authentication successful"
            };
        }
    }
}
