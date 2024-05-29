using MediatR;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsByCategory
{
    public class GetProductsByCategoryQueryRequest : IRequest<GetProductsByCategoryQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string CategoryId { get; set; }
    }
}