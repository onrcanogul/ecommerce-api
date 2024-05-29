using ECommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        
        readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            
            _productService = productService;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.UpdateProduct(new() {
               ProductId = request.Id,
               Name = request.Name,
               Price = request.Price,
               Stock = request.Stock,
               Categories = request.Categories
            });
            

            return new();
        }
    }
}
