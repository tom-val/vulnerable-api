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
[Route("v2/login")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AdminDbContext _context;
    public LoginController(AdminDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

        if (user is null)
        {
            return NotFound("Incorrect password or user not found.");
        }

        var passwordHash = PasswordHasher.ComputeHash(userLogin.Password, user.PasswordSalt);
        if (passwordHash == user.PasswordHash)
        {
            var token = GenerateToken(user);
            return Ok(token);
        }

        return NotFound("Incorrect password or user not found.");
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim("please_do_not_change_thanks", Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())))
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record UserLogin(string Email, string Password);
