using CashManiaAPI.Data.Models.Enums;

namespace CashManiaAPI.Data.Models.Entities;

public class Transaction
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; }

    //navigation properties
    public User User { get; set; }
}