using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AppUserCommands.GoogleLogin
{
    public class GoogleLoginCommandResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
