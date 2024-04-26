using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task DeleteProduct(string productId)
        {
            var product = await _productReadRepository.GetByIdAsync(productId);
            await _productWriteRepository.RemoveAsync(productId);
            await _productWriteRepository.SaveAsync();
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

        public async Task<ProductDto> GetProductById(string productId)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId, false);
            return new()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                CreatedDate = product.CreatedDate,
                ProductImageFiles = product.ProductImageFiles,
                Stock = product.Stock,
                UpdatedDate = product.UpdatedDate,
            };
        }

        public ListProduct GetProducts(int page, int size)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(page * size).Take(size).Include(p => p.ProductImageFiles).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles,
            }).ToList();
            return new()
            {
                Products = products,
                totalCount = totalCount
            };
        }

        public async Task UpdateProduct(string productId, string Name, float Price, int Stock)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId);
            product.Name = Name;
            product.Price = Price;
            product.Stock = Stock;
            _productWriteRepository.Update(product);
            await _productWriteRepository.SaveAsync();
        }
    }
}
