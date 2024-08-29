using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.DTOs;
using CashManiaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace CashManiaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(ITransactionService transactionService, IMapper mapper, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _mapper = mapper;
        _logger = logger;
    }

    // POST: api/transaction/add
    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("AddTransaction ModelState is invalid");
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.UserId = userId;

            await _transactionService.AddTransactionAsync(transaction);
            return Ok(new { message = "Transaction added successfully"});
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during AddTransaction");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: api/transaction/user-transactions
    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactions = await _transactionService.GetUserTransactionsAsync(userId);
            if (!transactions.Any())
            {
                _logger.LogInformation($"No transactions found for userId {userId}");
                return NotFound(new { message = "No transactions found for the user." });
            }

            var result = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during GetUserTransactions");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: api/transaction/get-filtered
    [HttpGet("get-filtered")]
    public async Task<IActionResult> GetFilteredTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate == null || endDate == null)
        {
            _logger.LogInformation("GetFilteredTransactions - StartDate or EndDate == null");
            return BadRequest("Both start date and end date are required.");
        }

        if (startDate > endDate)
        {
            _logger.LogInformation("GetFilteredTransactions - Incorrect date range");
            return BadRequest("Start date must be before end date.");
        }

        try
        {
            var transactions = await _transactionService.GetTransactionByDateRangeAsync(startDate, endDate);
            if (!transactions.Any())
                return NotFound("No transactions found for the given date range.");

            var result = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve transactions.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // DELETE: api/transaction/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
    {

        try
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogInformation($"DeleteTransaction - Transaction with ID {id} not found");
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (transaction.UserId != userId)
            {
                _logger.LogWarning($"User {userId} unauthorized to delete transaction with ID {id}");
                return Forbid();
            }

            await _transactionService.DeleteTransactionAsync(id);
            _logger.LogInformation($"Transaction with ID {id} deleted successfully");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred while deleting transaction with ID {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    // PUT: api/transaction/update
    [HttpPut("update")]
    public async Task<IActionResult> UpdateTransaction([FromBody] TransactionDto transactionDto)
    {
        if (!ModelState.IsValid || transactionDto.Id == 0)
        {
            _logger.LogWarning("UpdateTransaction ModelState is invalid");
            return BadRequest();
        }

        try
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(transactionDto.Id);
            if (transaction == null)
            {
                _logger.LogInformation($"Transaction with ID {transactionDto.Id} not found");
                return NotFound();
            }

            _mapper.Map(transactionDto, transaction);

            await _transactionService.UpdateTransactionAsync(transaction);
            _logger.LogInformation($"Transaction with ID {transactionDto.Id} updated successfully");
            return Ok(transaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occurred while updating transaction with ID {transactionDto.Id}");
            return StatusCode(500, "Internal server error");
        }
    }
}