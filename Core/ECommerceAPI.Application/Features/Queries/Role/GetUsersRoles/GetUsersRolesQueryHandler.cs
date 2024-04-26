using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Role.GetUsersRoles
{
    public class GetUsersRolesQueryHandler : IRequestHandler<GetUsersRolesQueryRequest, GetUsersRolesQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetUsersRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetUsersRolesQueryResponse> Handle(GetUsersRolesQueryRequest request, CancellationToken cancellationToken)
        {
           var result = await _roleService.GetUsersRoles(request.Id);
            return new()
            {
                Roles = result
            };
        }
    }
}
