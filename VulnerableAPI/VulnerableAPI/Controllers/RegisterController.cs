using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/register")]
public class RegisterController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AdminDbContext _context;
    private readonly IOptions<SqliteOptions> _options;

    public RegisterController(AdminDbContext context, IConfiguration config, IOptions<SqliteOptions> options)
    {
        _config = config;
        _context = context;
        _options = options;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] UserRegister userRegister)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == userRegister.Email);
        if (dbUser is not null)
        {
            return BadRequest("Email is already taken");
        }

        var salt = PasswordHasher.GenerateSalt();
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = userRegister.FirstName,
            LastName = userRegister.LastName,
            Email = userRegister.Email,
            PasswordSalt = salt,
            PasswordHash = PasswordHasher.ComputeHash(userRegister.Password, salt),
            IsAdmin = false
        };

        await using var dbContextTransaction = await _context.Database.BeginTransactionAsync();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Response.Headers.Add("RegisterFlag", _config["Flags:RegisterFlag"]);

        var userContext = new UserDbContext(_options, user.Id);
        await userContext.Database.MigrateAsync();
        userContext.Friends.Add(new Friend
        {
            Id = Guid.NewGuid(),
            FriendUserId = Guid.Parse("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d"),
            Name = "My best friend",
            UserId = user.Id
        });
        userContext.Friends.Add(new Friend
        {
            Id = Guid.NewGuid(),
            FriendUserId = Guid.Parse("133e3479-43b7-484a-8449-0ce31bc82f4c"),
            Name = "Kind of know him",
            UserId = user.Id
        });
        userContext.Users.Add(user);
        await userContext.SaveChangesAsync();

        await dbContextTransaction.CommitAsync();

        return Ok(new { userRegister.FirstName, userRegister.LastName, userRegister.Email });
    }
}

public record UserRegister(string FirstName, string LastName, string Email, string Password);
