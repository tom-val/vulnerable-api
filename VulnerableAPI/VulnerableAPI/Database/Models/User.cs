namespace VulnerableAPI.Database.Models;

public class User
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordSalt { get; set; }
    public required string PasswordHash { get; set; }
    public required bool IsAdmin { get; set; } = false;
}
