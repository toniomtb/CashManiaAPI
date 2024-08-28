using CashManiaAPI.Data.Models.Enums;

namespace CashManiaAPI.DTOs;
public class TransactionDto
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}