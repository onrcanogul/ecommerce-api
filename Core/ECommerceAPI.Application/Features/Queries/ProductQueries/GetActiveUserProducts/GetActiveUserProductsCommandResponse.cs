using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetActiveUserProducts
{
    public class GetActiveUsersProductsCommandResponse
    {
        public object Products { get; set; }
        public int TotalCount { get; set; }
    }
}
