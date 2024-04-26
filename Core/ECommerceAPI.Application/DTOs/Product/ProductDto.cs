using ECommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Product
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name{ get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles{ get; set; }
        //p.Id,
        //        p.Name,
        //        p.Stock,
        //        p.Price,
        //        p.CreatedDate,
        //        p.UpdatedDate,
        //        p.ProductImageFiles,
    }
}
