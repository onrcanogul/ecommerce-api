using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesCommandRequest : IRequest<GetRolesCommandResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}