using ECommerceAPI.Application.Abstractions.Hubs;
using ECommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductHubService _productHubService;
        readonly IProductService _productService;
        

        public CreateProductCommandHandler(IProductHubService productHubService,IProductService productService)
        {

            _productHubService = productHubService;
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productService.CreateProductAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            });
            
            await _productHubService.ProductAddedMessageAsync($"{request.Name} is added");
            return new();

        }
    }
}



//await _productWriteRepository.AddAsync(new() { Name = request.Name, Price = request.Price, Stock = request.Stock });
//await _productWriteRepository.SaveAsync();
