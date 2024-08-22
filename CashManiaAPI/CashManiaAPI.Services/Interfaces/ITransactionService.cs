using CashManiaAPI.Data.Models.Entities;

namespace CashManiaAPI.Services.Interfaces;

public interface ITransactionService
{
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string userId);
}