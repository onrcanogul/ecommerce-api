using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUserCommands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            Token token = await _authService.LoginAsync(new DTOs.User.Login {
                Password = request.Password , UsernameOrEmail =request.UsernameOrEmail 
            },900);

            return new LoginCommandSucceedResponse()
            {
                AccessToken = token.AccessToken,
                Expiration = token.Expiration,
                RefreshToken = token.RefreshToken
            };
            


            
     
        }
    }
}
