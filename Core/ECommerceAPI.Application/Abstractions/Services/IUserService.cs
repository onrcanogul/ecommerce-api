using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshTokenAsync(string refreshToken , AppUser user , DateTime accessTokenDate, int addToAccessToken);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<GetUsers> GetUsers(int page, int size);

        

        


    }
}
