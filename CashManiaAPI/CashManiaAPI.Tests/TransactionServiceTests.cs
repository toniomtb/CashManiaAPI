using CashManiaAPI.Data;
using CashManiaAPI.Data.Models.Entities;
using CashManiaAPI.Data.Models.Enums;
using CashManiaAPI.Services;
using FluentAssertions;
using Moq;

namespace CashManiaAPI.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _transactionService = new TransactionService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task AddTransactionAsync_Test()
        {
            // Arrange
            var transaction = new Transaction
            {
                Amount = 10, Date = DateTime.Now, Description = "Test add transaction", Type = TransactionType.Expense
            };

            _mockUnitOfWork.Setup(x => x.Transactions.AddAsync(transaction)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _transactionService.AddTransactionAsync(transaction);

            // Assert
            result.Should()?.BeEquivalentTo(transaction);
            _mockUnitOfWork.Verify(x => x.Transactions.AddAsync(transaction), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserTransactionsAsync_Test()
        {
            // Arrange
            var userId = "userId";
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, UserId = userId, Amount = 100 },
                new Transaction { Id = 2, UserId = userId, Amount = 200 }
            };
            _mockUnitOfWork.Setup(x => x.Transactions.GetTransactionsByUserIdAsync(userId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetUserTransactionsAsync(userId);

            // Assert
            result.Should()?.BeEquivalentTo(transactions);
            _mockUnitOfWork.Verify(x => x.Transactions.GetTransactionsByUserIdAsync(userId), Times.Once);
        }
    }
}