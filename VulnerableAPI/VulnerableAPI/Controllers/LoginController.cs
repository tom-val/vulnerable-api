using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VulnerableAPI.Database;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/login")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DatabaseContext _context;
    public LoginController(CreatedDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context.Context;
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
            var token = GenerateToken("admin");
            return Ok(token);
        }

        return NotFound("Incorrect password or user not found.");
    }

    private string GenerateToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name,username),
            //new Claim(ClaimTypes.Role,user.Role)
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
