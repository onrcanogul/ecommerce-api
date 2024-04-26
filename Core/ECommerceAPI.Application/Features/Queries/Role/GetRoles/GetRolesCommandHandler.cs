using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesCommandHandler : IRequestHandler<GetRolesCommandRequest, GetRolesCommandResponse>
    {
        private readonly IRoleService _roleService;

        public GetRolesCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesCommandResponse> Handle(GetRolesCommandRequest request, CancellationToken cancellationToken)
        {
            RoleDto roles = await _roleService.GetRoles(request.Page, request.Size);
            return new()
            {
                Roles = roles.Roles,
                TotalCount = roles.TotalCount
            };
        }
    }
}
