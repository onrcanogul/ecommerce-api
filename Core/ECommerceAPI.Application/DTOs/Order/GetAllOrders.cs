using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Order
{
    public class GetAllOrders
    {
        public int TotalCount { get; set; }
        public object Order { get; set; } //list<object>

    }
}
// 'orderCode', 'userName', 'totalPrice', 'createdDate'