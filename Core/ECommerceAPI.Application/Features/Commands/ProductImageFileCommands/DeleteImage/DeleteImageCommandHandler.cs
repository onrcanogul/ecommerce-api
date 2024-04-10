using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceAPI.Application.Features.Commands.ProductImageFileCommands.DeleteImage
{
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommandRequest, DeleteImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageWriteRepository;

        public DeleteImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productImageWriteRepository = productImageWriteRepository;
        }


        public async Task<DeleteImageCommandResponse> Handle(DeleteImageCommandRequest request, CancellationToken cancellationToken)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));
            ProductImageFile? image = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));
            product.ProductImageFiles.Remove(image);
            if(image != null)
            await _productImageWriteRepository.SaveAsync();

            return new();
        }
    }
}
