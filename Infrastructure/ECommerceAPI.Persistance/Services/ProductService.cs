using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Category;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistance.Repositories.Category;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerceAPI.Persistance.Services
{
    public class ProductService : IProductService
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        public ProductService(IHttpContextAccessor httpContextAccessor, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }

        private string getActiveUserId() =>  _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
  
        public async Task CreateProductAsync(CreateProduct model)
        {
            Category category =  await _categoryReadRepository.GetByIdAsync(model.CategoryId);

            Product product = new()
            {
                UserId = getActiveUserId(),
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            };
            product.Categories.Add(category);
            await _productWriteRepository.AddAsync(product);
            await _productWriteRepository.SaveAsync();
            
        }

        public async Task DeleteProduct(string productId)
        {
            var product = await _productReadRepository.GetByIdAsync(productId);
            if (product != null)
            {
                await _productWriteRepository.RemoveAsync(productId);
                await _productWriteRepository.SaveAsync();
            }
            else throw new BadHttpRequestException("product not found",400);
        }

        public async Task<GetProducts> GetActiveUsersProducts(int page, int size)
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

        public GetProducts GetProducts(int page, int size)
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

        public async Task UpdateProduct(UpdateProduct model)
        {
            
            Product product = await _productReadRepository.Table.Include(x => x.Categories).FirstOrDefaultAsync(p => p.Id == Guid.Parse(model.ProductId));

            product.Categories.Clear();

            foreach (var  categoryId in model.Categories) 
            {
                Category category = await _categoryReadRepository.GetByIdAsync(categoryId);
                product.Categories.Add(category);
            }
            
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            _productWriteRepository.Update(product);
            try
            {
                await _productWriteRepository.SaveAsync();
            }
            catch (Exception ex)
            {

            }
            
           
        }

        public async Task<GetProducts> GetProductsByCategory(int page, int size, string categoryId)
        {
            //totalcount
            var query = _categoryReadRepository.Table.Include(c => c.Products).ThenInclude(p => p.ProductImageFiles).Where(c => c.Id == Guid.Parse(categoryId));

            var category = await query.Skip(size * page).Take(size).GroupBy(n => n.Id).Select(group => group.First()).SingleOrDefaultAsync();
            var products = category.Products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles,
            });
            return new()
            {
                Products = products,
                totalCount = products.Count()
            };

        }

        //move to category
        public async Task<GetProducts> GetProductsWithPriceFilter(int page, int size, float max, float min, string? categoryId = null)
        {
            var query = _productReadRepository.Table.Include(x => x.ProductImageFiles).Where(x => x.Price <= max && x.Price >= min);
            List<Product> _products;
            if (categoryId != null)
            {
                Category? category = await _categoryReadRepository.Table.Include(x => x.Products).ThenInclude(x => x.ProductImageFiles).FirstOrDefaultAsync(x => x.Id == Guid.Parse(categoryId));
                if (category != null)
                    _products = category.Products.Where(x => x.Price<=max && x.Price>=min).ToList();
                else
                    throw new BadHttpRequestException("category not found", 404);              
                
            } else
                _products = await query.ToListAsync();

            _products = _products.Skip(size * page).Take(size).ToList();

            var products = _products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles,
            });

            return new()
            {
                Products = products,
                totalCount = query.Count()
            };


        }
    }
}
