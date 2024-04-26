using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Helpers
{
    public static class CustomEncoding
    {
        public static string Encode(this string value) 
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return WebEncoders.Base64UrlEncode(bytes); //urlde taşınabilir veri
            
        }
        public static string Decode(this string value)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
