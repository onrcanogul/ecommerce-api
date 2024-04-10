using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.Login;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IConfiguration _configuration;
        readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
        }


        private async Task<Token> ExternalUserLoginAsync(AppUser user,string email, string name,int accessTokenLifeTime, UserLoginInfo userLoginInfo)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                      user = new() 
                    { 
                        Id = Guid.NewGuid().ToString(),
                        Email = email, UserName = email,
                        NameSurname = name 
                    };
                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    result = createResult.Succeeded;
                    if (result)
                        await _userManager.AddLoginAsync(user, userLoginInfo);
                    else
                        throw new Exception("Invalid external authentication");
                }               
            }
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
            return new()
            {
                AccessToken = token.AccessToken,
                Expiration = token.Expiration,
            };

        }

        public async Task<Token> GoogleLoginAsync(string idToken,int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:ClientId"]  }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var userLoginInfo = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
            return await ExternalUserLoginAsync(user,payload.Email,payload.Name,accessTokenLifeTime,userLoginInfo);           
        }

        public async Task<Token> LoginAsync(Login model,int accessTokenLifeTime)
        {
            AppUser? user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            }
            if (user == null)
            {
                throw new NotFoundUserException();
            }
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return new Token
                {
                    AccessToken = token.AccessToken,
                    Expiration = token.Expiration
                };
            }
            throw new UserLoginErrorException();
        }
    }
}
