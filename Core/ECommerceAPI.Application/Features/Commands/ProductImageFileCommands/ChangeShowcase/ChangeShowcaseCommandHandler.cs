using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.ChangeShowcase
{
    public class ChangeShowcaseCommandHandler : IRequestHandler<ChangeShowcaseCommandRequest, ChangeShowcaseCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseCommandResponse> Handle(ChangeShowcaseCommandRequest request, CancellationToken cancellationToken)         
        {
            var query = _productImageFileWriteRepository.Table.Include(p => p.Products)
                   .SelectMany(p => p.Products, (pif, p) => new
                   {
                       pif,
                       p
                   });

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.ShowCase);
            if(data != null)
            data.pif.ShowCase = false;

            var image = await query.FirstOrDefaultAsync(pif => pif.pif.Id == Guid.Parse(request.ImageId));
            image.pif.ShowCase = true;

            await _productImageFileWriteRepository.SaveAsync();

            return new() { };

            
        }
    }
}
