using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashManiaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // POST: api/transaction/add
    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        transaction.UserId = User.Identity.Name;
        var addedTransaction = await _transactionService.AddTransactionAsync(transaction);

        return Ok(new { message = "Transaction added successfully", transaction = addedTransaction });
    }

    // GET: api/transaction/user-transactions
    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        var userId = User.Identity.Name;
        var transactions = await _transactionService.GetUserTransactionsAsync(userId);

        if (transactions == null || !transactions.Any())
            return NotFound(new { message = "No transactions found for the user." });

        return Ok(transactions);
    }
}
