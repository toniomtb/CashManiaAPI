using CashManiaAPI.Data.Repositories.Interfaces;

namespace CashManiaAPI.Data;

public interface IUnitOfWork : IDisposable
{
    ITransactionRepository Transactions { get; }
    ICategoryRepository Categories { get; }
    
    Task<int> SaveAsync();

    int Save();
}