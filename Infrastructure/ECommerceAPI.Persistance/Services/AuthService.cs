using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUserCommands.Login;
using ECommerceAPI.Application.Helpers;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
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
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, userLoginInfo);
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 50);
                return new() { AccessToken = token.AccessToken, Expiration = token.Expiration , RefreshToken=token.RefreshToken };

            }
            throw new Exception("Invalid external authentication.");



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
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 900);
                return new Token
                {
                    AccessToken = token.AccessToken,
                    Expiration = token.Expiration,
                    RefreshToken = token.RefreshToken
                };
            }
            throw new UserLoginErrorException();
        }

        public async Task<Token> RefreshTokenLogin(string refreshToken)
        {
          AppUser? user =  await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
          if(user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
          {
             Token token = _tokenHandler.CreateAccessToken(900,user);
             await _userService.UpdateRefreshTokenAsync(token.RefreshToken,user, token.Expiration, 50);
                return token;
          }
          else
            throw new NotFoundUserException();
        }

        public async Task PasswordResetAsync(string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if(user!=null)
            {
               string resetToken =  await _userManager.GeneratePasswordResetTokenAsync(user);
               resetToken = resetToken.Encode();
               
               await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                resetToken = resetToken.Decode();
                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }
    }
}
