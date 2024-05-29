using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Features.Commands.Category.CreateCategory;
using ECommerceAPI.Application.Features.Commands.Category.DeleteCategory;
using ECommerceAPI.Application.Features.Commands.Category.UpdateCategory;
using ECommerceAPI.Application.Features.Queries.Category.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery]GetCategoriesQueryRequest request)
        {
            GetCategoriesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequest request)
        {
            CreateCategoryCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryCommandRequest request)
        {
            UpdateCategoryCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute]DeleteCategoryCommandRequest request)
        {
            DeleteCategoryCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        
    }
}
