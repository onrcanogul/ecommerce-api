namespace ECommerceAPI.Application.Features.Queries.Order.GetActiveUsersOrders
{
    public class GetActiveUsersOrdersCommandResponse
    {
        public int TotalCount { get; set; }
        public object Orders { get; set; }
    }
}