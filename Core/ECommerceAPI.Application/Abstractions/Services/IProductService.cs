using ECommerceAPI.Application.DTOs.Product;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        GetProducts GetProducts(int page, int size);
        Task<ProductDto> GetProductById(string productId);
        Task DeleteProduct(string productId);
        Task UpdateProduct(UpdateProduct product);
        Task<GetProducts> GetActiveUsersProducts(int page, int size);
        Task<GetProducts> GetProductsByCategory(int page, int size,string categoryId);
        Task<GetProducts> GetProductsWithPriceFilter(int page, int size, float max, float min,string?categoryId = null);
    }
}
