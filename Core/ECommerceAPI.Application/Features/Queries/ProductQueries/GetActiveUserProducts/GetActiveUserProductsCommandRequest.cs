using MediatR;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetActiveUserProducts
{
    public class GetActiveUsersProductsCommandRequest : IRequest<GetActiveUsersProductsCommandResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}