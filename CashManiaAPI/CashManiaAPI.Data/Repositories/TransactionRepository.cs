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
}