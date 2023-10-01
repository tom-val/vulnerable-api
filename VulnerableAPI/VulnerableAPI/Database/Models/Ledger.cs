using VulnerableAPI.Database.Enums;

namespace VulnerableAPI.Database.Models;

public class Ledger
{
    public required Guid Id { get; set; }
    public required Currency Currency { get; set; }
    public required double Balance { get; set; }
    public required string Iban { get; set; }
    public required Guid UserId { get; set; }
    public User User { get; set; }
}
