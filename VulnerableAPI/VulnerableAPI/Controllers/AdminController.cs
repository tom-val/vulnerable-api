using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;

namespace VulnerableAPI.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("v2/admin")]
public class AdminController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AdminDbContext _context;
    public AdminController(AdminDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    [HttpGet("users")]
    public async Task<ActionResult> Login()
    {
        Response.Headers.Add("BrokenFunctionLevelAuthorization", _config["Flags:BrokenFunctionLevelAuthorization"]);
        var users = await _context.Users.Select(u => new { u.Email, u.FirstName, u.LastName }).ToListAsync();
        return Ok(users);
    }
}
