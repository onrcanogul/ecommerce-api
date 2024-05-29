using ECommerceAPI.Application.DTOs.Category;
using ECommerceAPI.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<GetCategories> GetAllCategoriesAsync(int page, int size);
        Task CreateCategoryAsync(string name);
        Task DeleteCategoryAsync(string id);
        Task UpdateCategoryAsync(string id, string name);
       
    }
}
