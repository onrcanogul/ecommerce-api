using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.CreateUser;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Persistance.Contexts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ECommerceAPIDbContext _dbContext;
        readonly RoleManager<AppRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, ECommerceAPIDbContext dbContext, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.Username,
                NameSurname = model.NameSurname
            }, model.Password);
            CreateUserResponse response = new() { Succeeded = result.Succeeded };


            if (result.Succeeded)
                response.Message = "User has been created succesfully";
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }



            return response;
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user , DateTime accessTokenDate, int addToAccesToken)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addToAccesToken);
                await _userManager.UpdateAsync(user);
            }
            else throw new NotFoundUserException();
           
        }
        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser? user =  await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                resetToken = resetToken.Decode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user); //tokenı geçersiz yapar
                }
                else
                    throw new PasswordChangeFailedException();
            } else
            throw new NotFoundUserException();
        }

        public async Task<GetUsers> GetUsers(int page, int size)
        {          
            var query = _userManager.Users.Select( u => new
            {
                Id = u.Id,
                Username = u.UserName,
                NameSurname = u.NameSurname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            });

            var datas = await query.Skip(size * page).Take(size).ToListAsync();

            return new()
            {
                TotalCount = query.Count(),
                Users = datas
            };
        }

        
    }
}
