using ECommerceAPI.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthService
    {
        Task<Application.DTOs.Token> LoginAsync(Login model , int accessTokenLifeTime);
        Task<Application.DTOs.Token> RefreshTokenLogin(string refreshToken);
    }
}
