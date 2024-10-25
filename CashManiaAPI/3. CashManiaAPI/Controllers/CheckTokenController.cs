using CashManiaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CashManiaAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class CheckTokenController : ControllerBase
{
    public CheckTokenController(ITransactionService transactionService, IMapper mapper, ILogger<TransactionController> logger)
    {
    }

    // GET: api/checkToken/checkTokenIsValid
    [HttpGet("checkTokenIsValid")]
    public async Task<IActionResult> CheckTokenIsValid()
    {
        return Ok();
    }
}