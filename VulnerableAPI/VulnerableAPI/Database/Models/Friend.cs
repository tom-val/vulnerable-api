namespace VulnerableAPI.Database.Models;

public class Friend
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public required Guid FriendUserId { get; set; }
    public User FriendUser { get; set; }
    public required Guid UserId { get; set; }
    public User User { get; set; }
}
