using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.User.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQueryRequest, GetUsersQueryResponse>
    {
        private readonly IUserService _userService;

        public GetUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var datas = await _userService.GetUsers(request.Page, request.Size);

            return new()
            {
                TotalCount = datas.TotalCount,
                Users = datas.Users,
            };
        }
    }
}
