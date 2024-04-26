using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.Order.CompleteOrder;
using ECommerceAPI.Application.Features.Commands.Order.CreateOrder;
using ECommerceAPI.Application.Features.Commands.Order.DeleteOrder;
using ECommerceAPI.Application.Features.Queries.Order.GetActiveUsersOrders;
using ECommerceAPI.Application.Features.Queries.Order.GetAllOrders;
using ECommerceAPI.Application.Features.Queries.Order.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    
    
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, Definition = "Create a order", Action = ActionType.Writing)]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
        {
            CreateOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, Definition = "Get all orders", Action = ActionType.Reading)]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest request)
        {
            GetAllOrdersQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin",Roles = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, Definition = "Get single order by id", Action = ActionType.Reading)]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest request)
        {
            GetOrderByIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("complete-order")]
        [Authorize(AuthenticationSchemes = "Admin", Roles = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, Definition = "Complete a order", Action = ActionType.Updating)]
        public async Task<IActionResult> CompleteOrder([FromBody]CompleteOrderCommandRequest request)
        {
            CompleteOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DeleteOrder([FromRoute]DeleteOrderCommandRequest request)
        {
            DeleteOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("active-users-orders")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetActiveUsersOrders([FromQuery] GetActiveUsersOrdersCommandRequest request)
        {
            GetActiveUsersOrdersCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
