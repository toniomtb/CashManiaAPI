using CashManiaAPI.Data.Models.Entities;
using System.Linq.Expressions;

namespace CashManiaAPI.Data.Repositories.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
    Task<bool> AnyAsync(Expression<Func<Transaction, bool>> predicate);
}