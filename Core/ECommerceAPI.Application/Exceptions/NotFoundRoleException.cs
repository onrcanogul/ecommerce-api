using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Exceptions
{
    public class NotFoundRoleException : Exception
    {
        public NotFoundRoleException(): base("Role has not found")
        {
        }

        public NotFoundRoleException(string? message) : base(message)
        {
        }

        public NotFoundRoleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
