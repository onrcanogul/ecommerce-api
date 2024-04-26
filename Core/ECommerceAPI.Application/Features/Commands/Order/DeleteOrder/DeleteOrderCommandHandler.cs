using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Order.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, DeleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<DeleteOrderCommandResponse> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.DeleteOrder(request.Id);
            return new();
        }
    }
}
