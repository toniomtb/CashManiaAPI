using System.Linq.Expressions;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CashManiaAPI.Data.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId)
    {
        return await _dbSet.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<IEnumerable<Transaction>> GetByDateFilteredAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Set<Transaction>().Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByDateFilteredSqlAsync(string userId, DateTime startDate, DateTime endDate)
    {
        endDate = endDate.Date.AddDays(1);
        var query = "SELECT * FROM Transactions WHERE UserId = @p0 AND Date >= @p1 AND Date < @p2";

        return await _context.Transactions
            .FromSqlRaw(query, userId, startDate, endDate)
            .AsNoTracking()
            .ToListAsync();
    }
}