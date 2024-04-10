using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUserCommands.Login
{
    public class LoginCommandResponse
    {
    }
    public class LoginCommandSucceedResponse : LoginCommandResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
    public class LoginCommandErrorResponse : LoginCommandResponse 
    {
        public string Message { get; set; }
    }
}
