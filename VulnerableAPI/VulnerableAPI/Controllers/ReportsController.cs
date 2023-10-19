using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/reports")]
public class ReportsController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public ReportsController(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpGet]
    public Task<List<ReportDto>> ReportedUsers()
    {
        return _context.Reports
            .Where(l => l.ReportedByUser.Email == User.GetEmail())
            .Select(report => new ReportDto(report.Id, report.ReportReason, report.ReportStatus, report.ReportedUser.FirstName))
            .ToListAsync();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ReportUser([FromBody] ReportUserDto reportDto)
    {
        var reportedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == reportDto.ReportUserEmail);
        if (reportedUser is null)
        {
            Response.Headers.Add("BrokenObjectPropertyAuthorizationExcessiveInformation", _config["Flags:BrokenObjectPropertyAuthorizationExcessiveInformation"]);
            return BadRequest("User does not exist in system.");
        }

        var report = new Report
        {
            Id = Guid.NewGuid(),
            ReportedByUserId = User.GetId(),
            ReportedUserId = reportedUser.Id,
            ReportReason = reportDto.ReportReason,
            ReportStatus = ReportStatus.Submitted
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        Response.Headers.Add("BrokenObjectPropertyAuthorizationExcessiveDataExposure", _config["Flags:BrokenObjectPropertyAuthorizationExcessiveDataExposure"]);
        return Ok(new { report.Id, report.ReportReason, report.ReportStatus, RportedUser = reportedUser.FirstName, LastKnownLocation = "Vilnius"});
    }

    public record ReportUserDto([MaxLength(500)] string ReportReason, [MaxLength(100)] string ReportUserEmail);
    public record ReportDto(Guid Id, string ReportReason, ReportStatus ReportStatus, string ReportedUser);
}
