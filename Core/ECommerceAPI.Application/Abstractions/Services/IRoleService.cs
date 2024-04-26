using ECommerceAPI.Application.DTOs.Role;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoles(int page, int size);
        Task<SingleRoleDto> GetRoleById(string id);
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string id);
        Task<bool> UpdateRole(string id,string name);
        Task<bool> AssignRole(string userId, List<string> roles);
        Task<string[]> GetUsersRoles(string userId);
           
    }
}
