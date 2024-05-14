using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Category : BaseEntity
    {
        public ICollection<Product> Products  { get; set; }
        public string Name { get; set; }
    }
}
