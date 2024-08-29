using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services;
using FluentAssertions;
using Moq;

namespace CashManiaAPI.Tests;

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _categoryService = new CategoryService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_Test()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Cat1" },
            new Category { Id = 2, Name = "Cat2" }
        };

        _mockUnitOfWork.Setup(x => x.Categories.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert 
        result.Should()?.BeEquivalentTo(categories);
        _mockUnitOfWork.Verify(x => x.Categories.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_Test()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Cat1" };
        _mockUnitOfWork.Setup(x => x.Categories.GetByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        result.Should()?.BeEquivalentTo(category);
        _mockUnitOfWork.Verify(x => x.Categories.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddCategoryAsync_Test()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Cat1" };

        _mockUnitOfWork.Setup(x => x.Categories.AddAsync(category)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1);

        // Act
        await _categoryService.AddCategoryAsync(category);

        // Assert
        _mockUnitOfWork.Verify(x => x.Categories.AddAsync(category), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_Test()
    {
        // Arrange
        var id = 1;
        var category = new Category { Id = id, Name = "Cat 1" };
        bool isDeleted = false;

        _mockUnitOfWork.Setup(x => x.Categories.GetByIdAsync(id))
            .ReturnsAsync(() => isDeleted ? null : category);
        _mockUnitOfWork.Setup(x => x.Transactions.AnyAsync(x => x.CategoryId == id)).ReturnsAsync(false);
        _mockUnitOfWork.Setup(x => x.Categories.Delete(category)).Callback(() => isDeleted = true);
        _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1);

        // Act
        await _categoryService.DeleteCategoryAsync(category);
        var resultGet = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        _mockUnitOfWork.Verify(x => x.Categories.Delete(category), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        resultGet.Should()?.BeNull();
    }
}