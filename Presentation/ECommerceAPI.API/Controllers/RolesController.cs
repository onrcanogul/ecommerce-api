using ECommerceAPI.Application.DTOs.Role;
using ECommerceAPI.Application.Features.Commands.Role.AssignRole;
using ECommerceAPI.Application.Features.Commands.Role.CreateRole;
using ECommerceAPI.Application.Features.Commands.Role.DeleteRole;
using ECommerceAPI.Application.Features.Commands.Role.UpdateRole;
using ECommerceAPI.Application.Features.Queries.Role.GetRoleById;
using ECommerceAPI.Application.Features.Queries.Role.GetRoles;
using ECommerceAPI.Application.Features.Queries.Role.GetUsersRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesCommandRequest request)
        {
            GetRolesCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetRoleById(GetRoleByIdCommandRequest request)
        {
            GetRoleByIdCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            CreateRoleCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommandRequest request)
        {
            UpdateRoleCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteRole([FromRoute]DeleteRoleCommandRequest request)
        {
            DeleteRoleCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody]AssignRoleCommandRequest request)
        {
            AssignRoleCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("users-roles")]
        public async Task<IActionResult> GetUsersRoles([FromQuery]GetUsersRolesQueryRequest request)
        {
            GetUsersRolesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
