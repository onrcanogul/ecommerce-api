using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Category.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryService _categoryService) : IRequestHandler<GetCategoriesQueryRequest, GetCategoriesQueryResponse>
    {
        public async Task<GetCategoriesQueryResponse> Handle(GetCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
           Application.DTOs.Category.GetCategories categories =  await _categoryService.GetAllCategoriesAsync(request.Page,request.Size);
            return new()
            {
                Categories = categories.Categories,
                TotalCount = categories.totalCount
            };
        }
    }
}
