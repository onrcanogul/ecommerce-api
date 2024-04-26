using MediatR;

namespace ECommerceAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandRequest : IRequest<CompleteOrderCommandResponse>
    {
        public string OrderId { get; set; }
    }
}