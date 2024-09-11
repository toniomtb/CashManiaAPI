using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Services.Interfaces;

namespace CashManiaAPI.Services;

public class TransactionService(IUnitOfWork unitOfWork) : ITransactionService
{
    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(transaction.CategoryId);
        if (category == null)
            transaction.CategoryId = 1; //force to the General category

        await unitOfWork.Transactions.AddAsync(transaction);
        await unitOfWork.SaveAsync();

        return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string userId)
    {
        return await unitOfWork.Transactions.GetTransactionsByUserIdAsync(userId);
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id)
    {
        return await unitOfWork.Transactions.GetByIdAsync(id);
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var transaction = await GetTransactionByIdAsync(id);
        if (transaction != null)
        {
            unitOfWork.Transactions.Delete(transaction);
            await unitOfWork.SaveAsync();
        }
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        unitOfWork.Transactions.Update(transaction);
        await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await unitOfWork.Transactions.GetByDateFilteredSqlAsync(startDate, endDate);
    }
}
