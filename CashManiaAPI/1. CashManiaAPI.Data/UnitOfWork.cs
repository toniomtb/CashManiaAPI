using CashManiaAPI.Data.Repositories.Interfaces;
using CashManiaAPI.Data.Repositories;

namespace CashManiaAPI.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private ITransactionRepository _transactionRepository;
    private ICategoryRepository _categoryRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ITransactionRepository Transactions => _transactionRepository ??= new TransactionRepository(_context);
    public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);

    public int Save()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
