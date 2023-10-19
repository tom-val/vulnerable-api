using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/profile")]
public class ProfileController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    private ILogger<ProfileController> _logger;
    public ProfileController(UserDbContext context, IConfiguration config, ILogger<ProfileController> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> AboutMe()
    {
        var user = await _context.Users.FirstAsync(u => u.Email == User.GetEmail());
        return Ok(new { user.FirstName, user.LastName, user.Email, user.IsAdmin });
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdate updateProfileDto)
    {
        if (updateProfileDto.FirstName.Length > 1000 || updateProfileDto.LastName.Length > 1000)
        {
            Response.Headers.Add("UnrestrictedConsumptionNoFieldLimit3", _config["Flags:UnrestrictedConsumptionNoFieldLimit(3)"]);
        }

        var result = await _context.Database.ExecuteSqlRawAsync(@$"
            UPDATE users
            SET firstName = ""{updateProfileDto.FirstName}"", lastName = ""{updateProfileDto.LastName}""
            WHERE email = ""{User.GetEmail()}""");

        await _context.SaveChangesAsync();

        if (result == 1)
        {
            var user = await _context.Users.FirstAsync(u => u.Email == User.GetEmail());

            if (user.IsAdmin)
            {
                Response.Headers.Add("SqlInjection", _config["Flags:SqlInjection"]);
            }

            return Ok(new { user.FirstName, user.LastName, user.Email, user.IsAdmin });
        }

        return BadRequest("Error while updating user.");
    }

    [Authorize]
    [HttpGet("picture")]
    public async Task<IActionResult> GetProfilePicture()
    {
        var path = $"{_config["FilesLocation"]}/{User.GetId()}.jpg";
        var fileName = Path.GetFileName(path);

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Cannot get picture before uploading.");
        }

        var content = await System.IO.File.ReadAllBytesAsync(path);
        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);

        var parsedString = Encoding.ASCII.GetString(content);
        if (parsedString.Contains("VulnerableAPI"))
        {
            Response.Headers.Add("ServerSideForgery", _config["Flags:ServerSideForgery"]);
        }

        return File(content, contentType, fileName);
    }

    [Authorize]
    [HttpPost("picture")]
    public async Task<IActionResult> UploadProfilePicture([FromBody] PictureDto pictureDto)
    {
        try
        {
            var fileContent = await DownloadFile(pictureDto.Url);

            if (fileContent is null)
            {
                return BadRequest("Cannot download image.");
            }

            if (fileContent.Length > 2000000)
            {
                return BadRequest("Max file size must be less than 2MB.");
            }

            await System.IO.File.WriteAllBytesAsync($"{_config["FilesLocation"]}/{User.GetId()}.jpg", fileContent);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save image.");
            return BadRequest("Cannot download image.");
        }
    }

    private static async Task<byte[]?> DownloadFile(string url)
    {
        using var client = new HttpClient();
        using var result = await client.GetAsync(url);
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadAsByteArrayAsync();
        }

        return null;
    }

    public record PictureDto(string Url);
    public record UserUpdate(string FirstName, string LastName);
}
