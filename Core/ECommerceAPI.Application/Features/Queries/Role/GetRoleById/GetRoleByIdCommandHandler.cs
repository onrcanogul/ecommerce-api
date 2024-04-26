using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Role;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Role.GetRoleById
{
    public class GetRoleByIdCommandHandler : IRequestHandler<GetRoleByIdCommandRequest, GetRoleByIdCommandResponse>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleByIdCommandResponse> Handle(GetRoleByIdCommandRequest request, CancellationToken cancellationToken)
        {
            SingleRoleDto role = await _roleService.GetRoleById(request.Id);
            return new()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}
