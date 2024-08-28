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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Unauthorized access to AddTransaction");
            return Unauthorized();
        }

        var transaction = _mapper.Map<Transaction>(transactionDto);
        transaction.UserId = userId;

        try
        {
            var addedTransaction = await _transactionService.AddTransactionAsync(transaction);
            return Ok(new { message = "Transaction added successfully", transaction = addedTransaction });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during AddTransaction");
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: api/transaction/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Unauthorized access attempt to DeleteTransaction");
            return Unauthorized();
        }

        try
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogInformation($"Transaction with ID {id} not found");
                return NotFound();
            }

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

        var transaction = await _transactionService.GetTransactionByIdAsync(transactionDto.Id);
        if (transaction == null)
        {
            _logger.LogInformation($"Transaction with ID {transactionDto.Id} not found");
            return NotFound();
        }

        _mapper.Map(transactionDto, transaction);

        try
        {
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

    // GET: api/transaction/user-transactions
    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Unauthorzied access to GetUserTransactions");
            return Unauthorized();
        }

        try
        {
            var transactions = await _transactionService.GetUserTransactionsAsync(userId);

            var result = _mapper.Map<IEnumerable<TransactionDto>>(transactions);

            if (!transactions.Any())
            {
                _logger.LogInformation($"No transactions found for userId {userId}");
                return NotFound(new { message = "No transactions found for the user." });
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred during GetUserTransactions");
            return StatusCode(500, "Internal server error");
        }
    }
}
