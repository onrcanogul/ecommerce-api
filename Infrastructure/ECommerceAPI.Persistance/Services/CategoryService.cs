using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Category;
using ECommerceAPI.Application.Repositories.Category;
using ECommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Services
{
    public class CategoryService(ICategoryWriteRepository _categoryWriteRepository, ICategoryReadRepository _categoryReadRepository) : ICategoryService
    {
        
        public async Task CreateCategoryAsync(string name)
        {
            await _categoryWriteRepository.AddAsync(new()
            {
                Name = name
            });
            await _categoryWriteRepository.SaveAsync();
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryWriteRepository.RemoveAsync(id);
            await _categoryWriteRepository.SaveAsync();
        }

        public async Task<GetCategories> GetAllCategoriesAsync()
        {
           List<Category> categories = await _categoryReadRepository.GetAll().Include(c => c.Products).ToListAsync();

            var newData = categories.Select(c => new
            {
                Name = c.Name,
                Products = c.Products,
                Id = c.Id 
            });

            return new()
            {
                Categories = newData,
                totalCount = categories.Count
            };
        }

        public async Task UpdateCategoryAsync(string id, string name)
        {
            Category category = await _categoryReadRepository.GetByIdAsync(id);
            category.Name = name;
            await _categoryWriteRepository.SaveAsync();
        }
    }
}
