namespace VulnerableAPI.Database.Models;

public class Referral
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool OpenedLink { get; set; }
    public User RefferedBy { get; set; }
    public Guid RefferedById { get; set; }
}
