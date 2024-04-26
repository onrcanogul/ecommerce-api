namespace ECommerceAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesCommandResponse
    {
        public int? TotalCount { get; set; }
        public object Roles { get; set; }
    }
}