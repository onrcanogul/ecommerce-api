using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Role;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistance.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly UserManager<AppUser> _userManager;
        readonly ECommerceAPIDbContext _dbContext;

        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, ECommerceAPIDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateRole(string name)
        {
           IdentityResult result =  await _roleManager.CreateAsync(new AppRole() { Id = Guid.NewGuid().ToString() ,Name = name });
           return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
           IdentityResult result =  await _roleManager.DeleteAsync(new() {Id = id});
            return result.Succeeded;
        }
        public async Task<bool> UpdateRole(string id, string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new() { Id = id, Name = name });
            return result.Succeeded;
        }
        public async Task<SingleRoleDto> GetRoleById(string id)
        {
            AppRole? role = await _roleManager.FindByIdAsync(id);
            if(role != null)
            {
                return new()
                {
                    Id = role.Id,
                    Name = role.Name,
                };
            }
            throw new NotFoundRoleException();
        }

        public async Task<RoleDto> GetRoles(int page, int size)
        {
            var query = _roleManager.Roles;
            
            List<AppRole> roles = await query.Skip(page*size).Take(size).ToListAsync();
            return new RoleDto()
            {
                Roles = roles,
                TotalCount = query.Count()
            };
            
        }

        public async Task<bool> AssignRole(string userId, List<string> roles)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await  _userManager.AddToRolesAsync(user, roles);
                return true;
            }
            return false; 
        }

        public async Task<string[]> GetUsersRoles(string userId)
        {
          var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
          if(user != null) 
          {
                var roles = await _userManager.GetRolesAsync(user);

                return roles.ToArray();
          } else
            throw new NotFoundUserException();
        }
    }
}
