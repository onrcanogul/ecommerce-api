using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using MediatR;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        
        readonly IProductService _productService;

        public GetByIdProductQueryHandler(IProductService productService)
        {
            
            _productService = productService;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            ProductDto product = await _productService.GetProductById(request.Id);
            return new()
            {
                Product = product
            };
            //Product product = await _productReadRepository.GetByIdAsync(request.Id, false);
            //return new() { Product = product};
        }
    }
}
