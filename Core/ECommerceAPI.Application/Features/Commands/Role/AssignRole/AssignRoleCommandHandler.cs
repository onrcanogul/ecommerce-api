using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Role.AssignRole
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
    {
        private readonly IRoleService _roleService;

        public AssignRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await _roleService.AssignRole(request.UserId, request.Roles);
            return new() { Result = result };
        }
    }
}
