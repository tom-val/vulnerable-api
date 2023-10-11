using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
public class ChangePasswordController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AdminDbContext _context;
    public ChangePasswordController(AdminDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("v2/change-password")]
    public async Task<ActionResult> Login([FromBody] UserChangePassword userLogin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

        if (user is null)
        {
            return NotFound("Incorrect password or user not found.");
        }

        var passwordHash = PasswordHasher.ComputeHash(userLogin.Password, user.PasswordSalt);
        if (passwordHash == user.PasswordHash)
        {
            user.PasswordHash = PasswordHasher.ComputeHash(userLogin.NewPassword, user.PasswordSalt);
            await _context.SaveChangesAsync();
            return Ok();
        }

        return NotFound("Incorrect password or user not found.");
    }

    [Authorize]
    [HttpPost("v1/change-password")]
    public async Task<ActionResult> Login([FromBody] UserChangePasswordOld userLogin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

        if (user is null)
        {
            return NotFound("Incorrect password or user not found.");
        }

        user.PasswordHash = PasswordHasher.ComputeHash(userLogin.NewPassword, user.PasswordSalt);
        await _context.SaveChangesAsync();

        Response.Headers.Add("ImproperInventoryManagement", _config["Flags:ImproperInventoryManagement"]);
        return Ok();
    }
}

public record UserChangePassword(string Email, string Password, string NewPassword);
public record UserChangePasswordOld(string Email, string NewPassword);
