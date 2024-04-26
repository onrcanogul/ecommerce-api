using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Features.Commands.AppUser.UpdatePassword;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.CreateUser;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.Login;
using ECommerceAPI.Application.Features.Queries.User.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {   
        readonly IMediator _mediator;
        readonly IMailService _mailService;
        public UsersController(IMediator mediator, IMailService mailService)
        {
            _mediator = mediator;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordCommandRequest request)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(request);          
             return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]GetUsersQueryRequest request)
        {
            GetUsersQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
