using CashManiaAPI.Data.Models.Enums;

namespace CashManiaAPI.DTOs;
public class TransactionDto
{
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}