using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/referrals")]
public class ReferralsController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public ReferralsController(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpGet("my-points")]
    public async Task<IActionResult> Points()
    {
        var referalsCount = await _context.Referrals
            .Where(l => l.RefferedBy.Email == User.GetEmail() && l.OpenedLink)
            .CountAsync();
        var points = referalsCount * 10;

        if (points > 1000)
        {
            Response.Headers.Add("UnrestrictedAccessSensitive", _config["Flags:UnrestrictedAccessSensitive"]);
        }

        return Ok(new PointsDto(points));
    }

    [Authorize]
    [HttpGet]
    public Task<List<ReferralDto>> GetReferrals()
    {
        return _context.Referrals
            .Where(l => l.RefferedBy.Email == User.GetEmail())
            .Select(referral => new ReferralDto(referral.Name, referral.Email, referral.OpenedLink))
            .ToListAsync();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Refer([FromBody] ReferUserDto referUserDto)
    {
        var existingReferral = await  _context.Referrals
            .Where(l => l.RefferedBy.Email == User.GetEmail())
            .FirstOrDefaultAsync(r => r.Email == referUserDto.Email);

        if (existingReferral is not null)
        {
            return BadRequest("Cannot refer same person.");
        }

        var referral = new Referral
        {
            Id = Guid.NewGuid(),
            RefferedById = User.GetId(),
            Email = referUserDto.Email,
            Name = referUserDto.Name,
            OpenedLink = false,
        };

        _context.Referrals.Add(referral);
        await _context.SaveChangesAsync();

        return Ok(new ReferUserResponseDto($"{HttpContext.Request.BaseUrl()}register-by-invite?referer={User.GetId()}&inviteId={referral.Id}"));
    }

    public record PointsDto(int Points);
    public record ReferUserDto([MaxLength(100)] string Name, [MaxLength(100)] [EmailAddress] string Email);
    public record ReferUserResponseDto(string Url);
    public record ReferralDto(string Name, string Email, bool OpenedLink);
}
