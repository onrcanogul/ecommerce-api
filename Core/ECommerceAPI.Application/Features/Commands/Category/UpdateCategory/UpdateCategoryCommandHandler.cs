using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Category.UpdateCategory
{
    public class UpdateCategoryCommandHandler(ICategoryService _categoryService) : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
    {
        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateCategoryAsync(request.Id, request.Name);
            return new();
        }
    }
}
