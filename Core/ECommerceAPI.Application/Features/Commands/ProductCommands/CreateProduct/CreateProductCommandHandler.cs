using ECommerceAPI.Application.Abstractions.Hubs;
using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.ProductCommands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductHubService _productHubService;
        readonly IProductService _productService;
        readonly IProductWriteRepository _productWriteRepository;

        public CreateProductCommandHandler(IProductHubService productHubService, IProductWriteRepository productWriteRepository,IProductService productService)
        {

            _productHubService = productHubService;
            _productWriteRepository = productWriteRepository;
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productService.CreateProductAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            });
            //await _productWriteRepository.AddAsync(new() { Name = request.Name, Price = request.Price, Stock = request.Stock });
            //await _productWriteRepository.SaveAsync();
            await _productHubService.ProductAddedMessageAsync($"{request.Name} is added");
            return new();

            //return new();

        }
    }
}



//await _productWriteRepository.AddAsync(new() { Name = request.Name, Price = request.Price, Stock = request.Stock });
//await _productWriteRepository.SaveAsync();
