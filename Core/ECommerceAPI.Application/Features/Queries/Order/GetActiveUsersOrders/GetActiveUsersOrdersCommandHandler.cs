using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Order.GetActiveUsersOrders
{
    public class GetActiveUsersOrdersCommandHandler : IRequestHandler<GetActiveUsersOrdersCommandRequest, GetActiveUsersOrdersCommandResponse>
    {
        private readonly IOrderService _orderService;

        public GetActiveUsersOrdersCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetActiveUsersOrdersCommandResponse> Handle(GetActiveUsersOrdersCommandRequest request, CancellationToken cancellationToken)
        {
            var datas = await _orderService.GetActiveUsersOrdersAsync(request.Page, request.Size);
            return new GetActiveUsersOrdersCommandResponse()
            {
                Orders = datas.Order,
                TotalCount = datas.TotalCount,
            };
        }
    }
}
