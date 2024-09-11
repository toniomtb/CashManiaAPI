using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services.Interfaces;

namespace CashManiaAPI.Services;

public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return unitOfWork.Categories.GetAllAsync();
    }

    public async Task AddCategoryAsync(Category category)
    {
        await unitOfWork.Categories.AddAsync(category);
        await unitOfWork.SaveAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await unitOfWork.Categories.GetByIdAsync(id);
    }

    public async Task DeleteCategoryAsync(Category category)
    { 
        if (await IsCategoryInUseAsync(category.Id))
            throw new InvalidOperationException("Category cannot be deleted because it is in use.");

        unitOfWork.Categories.Delete(category);
        await unitOfWork.SaveAsync();
    }

    private async Task<bool> IsCategoryInUseAsync(int categoryId)
    {
        return await unitOfWork.Transactions.AnyAsync(x => x.CategoryId == categoryId);
    }
}
