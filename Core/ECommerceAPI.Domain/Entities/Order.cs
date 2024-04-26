using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Entities.Common;

namespace ECommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public Basket Basket { get; set; }
        public CompletedOrder CompletedOrder { get; set; }
    }
}
