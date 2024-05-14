using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetActiveUserProducts
{
    internal class GetActiveUsersProductsCommandHandler : IRequestHandler<GetActiveUsersProductsCommandRequest, GetActiveUsersProductsCommandResponse>
    {
        private readonly IProductService _productService;

        public GetActiveUsersProductsCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetActiveUsersProductsCommandResponse> Handle(GetActiveUsersProductsCommandRequest request, CancellationToken cancellationToken)
        {
          GetProducts datas = await _productService.GetActiveUsersProducts(request.Page, request.Size);
            return new()
            {
                Products = datas.Products,
                TotalCount = datas.totalCount
            };
        }
    }
}
