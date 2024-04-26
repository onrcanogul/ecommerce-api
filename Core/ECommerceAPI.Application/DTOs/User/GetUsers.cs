using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.User
{
    public class GetUsers
    {
        public int TotalCount { get; set; }
        public object Users { get; set; }
    }
}
