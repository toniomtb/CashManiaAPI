using CashManiaAPI.Data.Models.Entities;

namespace CashManiaAPI.Data.Repositories.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
}