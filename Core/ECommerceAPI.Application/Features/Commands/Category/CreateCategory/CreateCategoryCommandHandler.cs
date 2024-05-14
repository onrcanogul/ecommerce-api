using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandHandler(ICategoryService _categoryService) : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.CreateCategoryAsync(request.Name);
            return new();
        }
    }
}
