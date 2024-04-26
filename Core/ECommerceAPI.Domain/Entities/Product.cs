using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        //public string? UserId { get; set; }
        //public AppUser? User { get; set; }
        public string UserId { get; set; }
        public AppUser User{ get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }

    }
}
