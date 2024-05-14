using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler(ICategoryService _categoryService) : IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
    {
        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(request.Id);
            return new();
        }
    }
}
