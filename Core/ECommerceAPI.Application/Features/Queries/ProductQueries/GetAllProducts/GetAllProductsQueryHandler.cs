using ECommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        
        readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            
            _productService = productService;
        }
        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = _productService.GetProducts(request.Page,request.Size);

            //var totalCount = _productReadRepository.GetAll(false).Count();
            //var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Include(p => p.ProductImageFiles).Select(p => new
            //{
            //    p.Id,
            //    p.Name,
            //    p.Stock,
            //    p.Price,
            //    p.CreatedDate,
            //    p.UpdatedDate,
            //    p.ProductImageFiles,
                
            //}).ToList();
            return new()
            {
                Products = products.Products,
                TotalCount = products.totalCount,
            };
        }
    }
}
