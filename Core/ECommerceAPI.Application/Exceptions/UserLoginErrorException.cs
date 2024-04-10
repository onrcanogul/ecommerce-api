using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Exceptions
{
    public class UserLoginErrorException : Exception
    {
        public UserLoginErrorException()
        {
        }

        public UserLoginErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserLoginErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
