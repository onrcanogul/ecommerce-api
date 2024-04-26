using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Role.GetRoleById
{
    public class GetRoleByIdCommandRequest : IRequest<GetRoleByIdCommandResponse>
    {
        public string Id { get; set; }
    }
}