using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/register")]
public class RegisterController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DatabaseContext _context;
    public RegisterController(CreatedDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context.Context;
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
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Response.Headers.Add("RegisterFlag", _config["Flags:RegisterFlag"]);

        return Ok(new {userRegister.FirstName, userRegister.LastName, userRegister.Email});
    }
}



public record UserRegister(string FirstName, string LastName, string Email, string Password);
