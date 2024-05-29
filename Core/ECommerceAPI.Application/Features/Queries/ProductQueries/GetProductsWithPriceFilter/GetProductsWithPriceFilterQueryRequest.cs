using MediatR;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetProductsWithPriceFilter
{
    public class GetProductsWithPriceFilterQueryRequest : IRequest<GetProductsWithPriceFilterQueryResponse>
    {
        public float Max { get; set; }
        public float Min { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public string? CategoryId { get; set; }
    }
}