using ECommerceAPI.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task CreateProductAsync(CreateProduct product);
        Task GetProducts();
        Task GetProductById(string productId);
        Task DeleteProduct(string productId);
        Task UpdateProduct(string productId);
        Task<ListProduct> GetActiveUsersProducts(int page, int size);
    }
}
