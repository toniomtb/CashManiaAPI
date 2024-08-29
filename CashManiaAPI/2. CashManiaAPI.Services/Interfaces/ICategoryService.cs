using CashManiaAPI.Data.Models.Entities;

namespace CashManiaAPI.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(int id);
    Task DeleteCategoryAsync(Category category);
}