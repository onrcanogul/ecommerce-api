using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        
        readonly IProductService _productService;


        public DeleteProductCommandHandler(IProductService productService)
        {
            
            _productService = productService;
        }


        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProduct(request.id);
            return new();
        }
    }
}
