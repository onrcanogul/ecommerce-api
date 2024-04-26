using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public ProductService(IHttpContextAccessor httpContextAccessor, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        private string getActiveUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task CreateProductAsync(CreateProduct product)
        {
            
            await _productWriteRepository.AddAsync(new()
            {
                UserId = getActiveUserId(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });
            await _productWriteRepository.SaveAsync();
            
        }

        public Task DeleteProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ListProduct> GetActiveUsersProducts(int page, int size)
        {
            var query = _productReadRepository.Table.Include(x => x.User).Where(p => p.UserId == getActiveUserId());
            var datas = await query.Skip(page * size).Take(size).Select(x => new
            {
                CreatedDate = x.CreatedDate,
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ProductImageFiles = x.ProductImageFiles,
                Stock = x.Stock,
                UpdatedDate = x.UpdatedDate
            }).ToListAsync();
            return new()
            {
                Products = datas,
                totalCount = query.Count()
            };          
        }

        public async Task GetProductById(string productId)
        {
            throw new Exception();

        }

        public Task GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
