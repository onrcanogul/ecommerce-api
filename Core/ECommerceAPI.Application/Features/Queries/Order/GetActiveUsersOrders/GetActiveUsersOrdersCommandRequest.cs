using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Order.GetActiveUsersOrders
{
    public class GetActiveUsersOrdersCommandRequest : IRequest<GetActiveUsersOrdersCommandResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}