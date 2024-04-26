using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
           var order = await _orderService.GetOrderByIdAsync(request.Id);
            return new()
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                Description = order.Description,
                Address = order.Address,
                BasketItems = order.BasketItems,
                CreatedDate = order.CreatedDate,
                Completed = order.Completed
            };
        }
    }
}
