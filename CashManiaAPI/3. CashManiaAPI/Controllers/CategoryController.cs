using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.DTOs;
using CashManiaAPI.Services.Interfaces;

namespace CashManiaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionController> _logger;

    public CategoryController(IMapper mapper, ILogger<TransactionController> logger, ICategoryService categoryService)
    {
        _mapper = mapper;
        _logger = logger;
        _categoryService = categoryService;
    }

    // GET: api/category/get-all
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (!categories.Any())
            {
                _logger.LogInformation("No categories found");
                return Ok(new { message = "No categories found" });
            }

            var result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during GetAllCategories");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/category/add
    [HttpPost("add")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("AddCategory ModelState is invalid");
            return BadRequest(ModelState);
        }

        try
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryService.AddCategoryAsync(category);
            return Ok(new { message = "Category added successfully" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during AddCategory");
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: api/category/delete
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _logger.LogInformation($"DeleteCategory - Category with ID {id} not found");
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(category);
            _logger.LogInformation($"Category with ID {id} deleted successfully");
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Exception occurred during DeleteCategory");
            return BadRequest(e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during DeleteCategory");
            return StatusCode(500, "Internal server error");
        }
    }
}