using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult Login([FromBody] UserLogin userLogin)
    {
        if (userLogin.Username == "admin" && userLogin.Password == "test")
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
            new Claim(ClaimTypes.NameIdentifier,username),
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

public record UserLogin(string Username, string Password);
