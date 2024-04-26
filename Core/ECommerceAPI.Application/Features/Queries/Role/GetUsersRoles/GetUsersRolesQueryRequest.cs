using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Role.GetUsersRoles
{
    public class GetUsersRolesQueryRequest : IRequest<GetUsersRolesQueryResponse>
    {
        public string Id { get; set; }
    }
}