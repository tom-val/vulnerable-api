using VulnerableAPI.Database.Enums;

namespace VulnerableAPI.Database.Models;

public class MoneyRequest
{
    public Guid Id { get; set; }
    public required MoneyRequestStatus Status { get; set; }
    public required Guid RequestedById { get; set; }
    public User RequestedBy { get; set; }
    public required Guid RequestedFromId { get; set; }
    public User RequestedFrom { get; set; }
    public double Amount { get; set; }
    public Currency Currency { get; set; }
    public string RequestReason { get; set; }
}

public enum MoneyRequestStatus
{
    Requested,
    Approved,
    Declined
}
