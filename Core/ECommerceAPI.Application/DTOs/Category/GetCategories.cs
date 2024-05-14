using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Category
{
    public class GetCategories
    {
        public object Categories { get; set; }
        public int totalCount { get; set; }
    }
}
