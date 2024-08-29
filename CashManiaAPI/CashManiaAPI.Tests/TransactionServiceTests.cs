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

        [Fact]
        public async Task GetTransactionByIdAsync_Test()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Amount = 100 };
            _mockUnitOfWork.Setup(x => x.Transactions.GetByIdAsync(1))
                .ReturnsAsync(transaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            result.Should()?.BeEquivalentTo(transaction);
            _mockUnitOfWork.Verify(x => x.Transactions.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTransactionAsync_Test()
        {
            // Arrange
            var id = 1;
            var transaction = new Transaction { Id = id, Amount = 100 };
            bool isDeleted = false;

            _mockUnitOfWork.Setup(x => x.Transactions.GetByIdAsync(id))
                .ReturnsAsync(() => isDeleted ? null : transaction);
            _mockUnitOfWork.Setup(x => x.Transactions.Delete(transaction)).Callback(() => isDeleted = true).Verifiable();
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1).Verifiable();

            // Act 
            await _transactionService.DeleteTransactionAsync(id);
            var resultGet = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            _mockUnitOfWork.Verify(x => x.Transactions.Delete(transaction), Times.Once());
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
            resultGet.Should()?.BeNull();
        }

        [Fact]
        public async Task UpdateTransactionAsync_Test()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, Amount = 100 };
            var updatedTransaction = new Transaction { Id = 1, Amount = 200 };

            _mockUnitOfWork.Setup(x => x.Transactions.Update(updatedTransaction)).Callback(() =>
            {
                transaction.Amount = 200;
            }).Verifiable();
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1).Verifiable();

            // Act
            await _transactionService.UpdateTransactionAsync(updatedTransaction);

            // Assert
            _mockUnitOfWork.Verify(x => x.Transactions.Update(updatedTransaction), Times.Once());
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
            transaction.Amount.Should().Be(200);
        }
    }
}