using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Product
{
    public class GetProducts
    {
        public object Products { get; set; }
        public int totalCount { get; set; }
        //public string Id { get; set; }
        //public int Stock { get; set; }
        //public float Price { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public ICollection<ProductImageFile> ProductImageFiles { get; set; }


    }
}
                