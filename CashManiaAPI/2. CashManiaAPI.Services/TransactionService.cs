using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services.Interfaces;

namespace CashManiaAPI.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        await _unitOfWork.Transactions.AddAsync(transaction);
        await _unitOfWork.SaveAsync();

        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string userId)
    {
        return await _unitOfWork.Transactions.GetTransactionsByUserIdAsync(userId);
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await _unitOfWork.Transactions.GetByIdAsync(id);
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var transaction = await GetTransactionByIdAsync(id);
        if (transaction != null)
        {
            _unitOfWork.Transactions.Delete(transaction);
            await _unitOfWork.SaveAsync();
        }
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        _unitOfWork.Transactions.Update(transaction);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Transactions.GetByDateFilteredAsync(startDate, endDate);
    }
}
