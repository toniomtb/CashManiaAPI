using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services.Interfaces;

namespace CashManiaAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return _unitOfWork.Categories.GetAllAsync();
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _unitOfWork.Categories.GetByIdAsync(id);
    }

    public async Task DeleteCategoryAsync(Category category)
    { 
        if (await IsCategoryInUseAsync(category.Id))
            throw new InvalidOperationException("Category cannot be deleted because it is in use.");

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveAsync();
    }

    private async Task<bool> IsCategoryInUseAsync(int categoryId)
    {
        return await _unitOfWork.Transactions.AnyAsync(x => x.CategoryId == categoryId);
    }
}
