using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsWithPriceFilter
{
    public class GetProductsWithPriceFilterQueryHandler(IProductService productService) : IRequestHandler<GetProductsWithPriceFilterQueryRequest, GetProductsWithPriceFilterQueryResponse>
    {
        public async Task<GetProductsWithPriceFilterQueryResponse> Handle(GetProductsWithPriceFilterQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await productService.GetProductsWithPriceFilter(request.Page, request.Size, request.Max, request.Min, request.CategoryId);

            return new()
            {
                Products = products.Products,
                TotalCount = products.totalCount
            };
        }
    }
}
