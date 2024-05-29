using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Product
{
    public class UpdateProduct
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        public List<string> Categories { get; set; }
    }

    //string productId, string Name, float Price, int Stock
}
