using CashManiaAPI.Data.Models.Entities;

namespace CashManiaAPI.Services.Interfaces;

public interface ITransactionService
{
    Task<Transaction> AddTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string userId);
    Task<Transaction> GetTransactionByIdAsync(int id);
    Task DeleteTransactionAsync(int id);
    Task UpdateTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionByDateRangeAsync(DateTime startDate, DateTime endDate);
}