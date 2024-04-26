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
            await _productService.UpdateProduct(request.Id, request.Name, request.Price, request.Stock);
            //Product product = await _productReadRepository.GetByIdAsync(request.Id);
            //product.Name = request.Name;
            //product.Price = request.Price;
            //product.Stock = request.Stock;
            //_productWriteRepository.Update(product);
            //await _productWriteRepository.SaveAsync();

            return new();
        }
    }
}
