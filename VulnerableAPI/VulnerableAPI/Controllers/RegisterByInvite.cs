using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("register-by-invite")]
public class RegisterByInviteController : ControllerBase
{

    private readonly AdminDbContext _context;
    private readonly IOptions<SqliteOptions> _options;

    public RegisterByInviteController(AdminDbContext context, IOptions<SqliteOptions> options)
    {
        _context = context;
        _options = options;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> RegisterByInvite([FromQuery] Guid referer, Guid inviteId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == referer);
        if (user is null)
        {
            return Content("<html><p><i>Hello! Thanks for accepting invitation!</i></p></html>", "text/html");
        }

        var userContext = new UserDbContext(_options, user.Id);
        var invite = await userContext.Referrals.FirstOrDefaultAsync(i => i.Id == inviteId);

        if (invite is null)
        {
            return Content("<html><p><i>Hello! Thanks for accepting invitation!</i></p></html>", "text/html");
        }

        invite.OpenedLink = true;
        await userContext.SaveChangesAsync();

        return Content($"<html><p><i>Hello! Thanks for accepting invitation!</i></p> <p> Invited by user <b>{user.FirstName}</b></p></html>", "text/html");
    }
}
