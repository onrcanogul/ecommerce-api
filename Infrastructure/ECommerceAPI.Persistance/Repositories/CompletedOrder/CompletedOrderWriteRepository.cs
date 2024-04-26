using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Repositories
{
    internal class CompletedOrderWriteRepository : WriteRepository<Domain.Entities.CompletedOrder>, ICompletedOrderWriteRepository
    {
        public CompletedOrderWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
