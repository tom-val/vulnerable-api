namespace VulnerableAPI.Database.Models;

public class Ledger
{
    public required Guid Id { get; set; }
    public required Currency Currency { get; set; }
    public required double Balance { get; set; }
}

public enum Currency
{
    EUR,
    USD
}
