namespace VulnerableAPI.Database.Models;

public class Report
{
    public Guid Id { get; set; }
    public string ReportReason { get; set; }
    public ReportStatus ReportStatus { get; set; }
    public User ReportedUser { get; set; }
    public Guid ReportedUserId { get; set; }
    public User ReportedByUser { get; set; }
    public Guid ReportedByUserId { get; set; }
}

public enum ReportStatus
{
    Submitted,
    Reviewed,
    ActionTaken,
    Declined
}
