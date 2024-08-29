namespace CashManiaAPI.Data.Models.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<Transaction> Transactions { get; set; }
}