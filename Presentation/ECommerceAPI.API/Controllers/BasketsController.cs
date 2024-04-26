using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Basket.AddItemToBasket;
using ECommerceAPI.Application.Features.Commands.Basket.RemoveBasketItem;
using ECommerceAPI.Application.Features.Commands.Basket.UpdateQuantity;
using ECommerceAPI.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets , Definition = "Get all baskets", Action = ActionType.Reading)]
        public async Task<IActionResult> GetBasketItems([FromQuery]GetBasketItemsQueryRequest request)
        {
           List<GetBasketItemsQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, Definition = "Add to basket", Action = ActionType.Writing)]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
        {
            AddItemToBasketCommandResponse response  = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, Definition = "Update Quantity", Action = ActionType.Updating)]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest reqeust)
        {
            UpdateQuantityCommandResponse response = await _mediator.Send(reqeust);
            return Ok(response);
        }
        [HttpDelete("{BasketItemId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, Definition = "Remove a basket", Action = ActionType.Deleting)]
        public async Task<IActionResult> RemoveBasketItem([FromRoute]RemoveBasketItemCommandRequest request)
        {
            RemoveBasketItemCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
