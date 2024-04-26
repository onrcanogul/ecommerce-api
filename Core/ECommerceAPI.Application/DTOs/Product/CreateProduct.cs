using Google.Apis.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Product
{
    public class CreateProduct
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
    }
}
