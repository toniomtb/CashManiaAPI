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

    public TransactionController(ITransactionService transactionService, IMapper mapper)
    {
        _transactionService = transactionService;
        _mapper = mapper;
    }

    // POST: api/transaction/add
    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var transaction = _mapper.Map<Transaction>(transactionDto);
        transaction.UserId = userId;

        var addedTransaction = await _transactionService.AddTransactionAsync(transaction);

        return Ok(new { message = "Transaction added successfully", transaction = addedTransaction });
    }

    // GET: api/transaction/user-transactions
    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var transactions = await _transactionService.GetUserTransactionsAsync(userId);

        var result = _mapper.Map<IEnumerable<TransactionDto>>(transactions);

        if (!transactions.Any())
            return NotFound(new { message = "No transactions found for the user." });

        return Ok(result);
    }
}
