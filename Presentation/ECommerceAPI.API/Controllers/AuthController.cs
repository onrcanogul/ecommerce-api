using ECommerceAPI.Application.Features.Commands.AppUser.PasswordReset;
using ECommerceAPI.Application.Features.Commands.AppUser.VerifyResetToken;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.Login;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCommandRequest request)
        {
            LoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest request)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest request)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(PasswordResetCommandRequest request)
        {
            PasswordResetCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody]VerifyResetTokenCommandRequest request)
        {
            VerifyResetTokenCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
