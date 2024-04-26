using ECommerceAPI.Application.DTOs.Role;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Application.Features.Queries.Role.GetUsersRoles
{
    public class GetUsersRolesQueryResponse
    {
        public string[] Roles { get; set; }
    }
}