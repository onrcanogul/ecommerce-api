using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Category;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Category;
using ECommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Persistance.Services
{
    public class CategoryService(ICategoryWriteRepository _categoryWriteRepository, ICategoryReadRepository _categoryReadRepository,
        IProductReadRepository _productReadRepository) : ICategoryService
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

        

        public async Task<GetCategories> GetAllCategoriesAsync(int page, int size)
        {
            var query = _categoryReadRepository.GetAll();

            List<Category> categories = null;

            if(page == -1 || size == -1) 
                categories = await query.ToListAsync();       
            else          
                categories = await query.Skip(page * size).Take(size).ToListAsync();
            

            var newData = categories.Select(c => new
            {
                Name = c.Name,
                Id = c.Id 
            });

            return new()
            {
                Categories = newData,
                totalCount = query.Count()
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
