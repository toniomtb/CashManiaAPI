using CashManiaAPI.Data.Repositories.Interfaces;
using CashManiaAPI.Data.Repositories;

namespace CashManiaAPI.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private ITransactionRepository _transactionRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ITransactionRepository Transactions => _transactionRepository ??= new TransactionRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
