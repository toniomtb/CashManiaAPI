using Microsoft.AspNetCore.Identity;

namespace CashManiaAPI.Data.Models.Entities;

public class User : IdentityUser
{
    //navigation properties
    public ICollection<Transaction> Transactions { get; set; }
}