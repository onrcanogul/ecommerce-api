using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Category.GetCategories
{
    public class GetCategoriesQueryRequest : IRequest<GetCategoriesQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}