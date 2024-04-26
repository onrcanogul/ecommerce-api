using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
        readonly UserManager<AppUser> _userManager;

        public TokenHandler(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public  Application.DTOs.Token CreateAccessToken(int second,AppUser user)
        {
            Application.DTOs.Token token = new();
            var roles = _userManager.GetRolesAsync(user).Result;
            string role = "";
            if (roles.Any())
            {
                role = roles[0];
            }

            //security->symetric
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //new key
            SigningCredentials signingCredentials = new(securityKey,SecurityAlgorithms.HmacSha256);

            //options
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken jwtSecurityToken = new(
                issuer: _configuration["Token:Issuer"],
                audience : _configuration["Token:Audience"],
                notBefore : DateTime.UtcNow, //simdi baslat
                expires:token.Expiration,
                signingCredentials : signingCredentials,
                claims :new List<Claim> {new(ClaimTypes.Name , user.UserName), new(ClaimTypes.NameIdentifier, user.Id), new("role",role),new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())}
                );

            //an instance of tokencreater
            JwtSecurityTokenHandler tokenHandler = new();

            
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
