using BPT.FMS.Application.Features.User.Commands;
using BPT.FMS.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPT.FMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            if (!result.IsSuccess)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                token = result.Token,
                admin = new
                {
                    result.User?.Id,
                    result.User?.Email,
                    result.User?.UserName,
                    result.User?.AccessLevel
                }
            });
        }
    }
}
