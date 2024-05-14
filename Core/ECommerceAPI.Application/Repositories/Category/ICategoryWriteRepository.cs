using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories.Category
{
    public interface ICategoryWriteRepository : IWriteRepository<Domain.Entities.Category>
    {
    }
}
