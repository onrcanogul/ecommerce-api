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
        ListProduct GetProducts(int page, int size);
        Task<ProductDto> GetProductById(string productId);
        Task DeleteProduct(string productId);
        Task UpdateProduct(string productId, string Name, float Price, int Stock);
        Task<ListProduct> GetActiveUsersProducts(int page, int size);
    }
}
