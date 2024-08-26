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
}
