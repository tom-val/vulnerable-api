namespace VulnerableAPI.Database.Models;

public class MoneyRequest
{
    public Guid Id { get; set; }
    public required MoneyRequestStatus Status { get; set; }
    public required Guid RequestedById { get; set; }
    public User RequestedBy { get; set; }
    public required Guid RequestedFromId { get; set; }
    public User RequestedFrom { get; set; }
}

public enum MoneyRequestStatus
{
    Requested,
    Approved,
    Declined
}
