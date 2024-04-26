using MediatR;

namespace ECommerceAPI.Application.Features.Queries.User.GetUsers
{
    public class GetUsersQueryRequest : IRequest<GetUsersQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}