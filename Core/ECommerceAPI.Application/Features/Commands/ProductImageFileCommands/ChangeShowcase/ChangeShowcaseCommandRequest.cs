using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.ChangeShowcase
{
    public class ChangeShowcaseCommandRequest : IRequest<ChangeShowcaseCommandResponse>
    {
        public string ImageId { get; set; }
        public string ProductId { get; set; }
    }
}
