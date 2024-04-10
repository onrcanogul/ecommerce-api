using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services.Authentication
{
    public interface IExternalAuthService
    {
        Task<Application.DTOs.Token> GoogleLoginAsync(string idToken,int accessTokenLifeTime);
        //Task FacebookLoginAsync();
    }
}
