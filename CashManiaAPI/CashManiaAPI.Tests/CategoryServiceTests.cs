using CashManiaAPI.Data;
using CashManiaAPI.Services;
using Moq;

namespace CashManiaAPI.Tests;

class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _categoryService = new CategoryService(_mockUnitOfWork.Object);
    }
}