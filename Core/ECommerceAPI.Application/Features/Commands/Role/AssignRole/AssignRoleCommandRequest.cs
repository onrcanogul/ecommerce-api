using MediatR;

namespace ECommerceAPI.Application.Features.Commands.Role.AssignRole
{
    public class AssignRoleCommandRequest : IRequest<AssignRoleCommandResponse>
    {
        public string UserId { get; set; }  
        public List<string> Roles { get; set; }
    }
}