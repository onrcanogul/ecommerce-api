using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsByCategory
{
    public class GetProductsByCategoryQueryHandler(IProductService productService) : IRequestHandler<GetProductsByCategoryQueryRequest, GetProductsByCategoryQueryResponse>
    {
        public async Task<GetProductsByCategoryQueryResponse> Handle(GetProductsByCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            GetProducts products = await productService.GetProductsByCategory(request.Page, request.Size, request.CategoryId);
            return new()
            {
                Products = products.Products,
                TotalCount = products.totalCount
            };
         }
    }
}
