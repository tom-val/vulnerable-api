using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
    [HttpGet("picture")]
    public async Task<IActionResult> GetProfilePicture()
    {
        var path = $"{_config["FilesLocation"]}/{User.GetId()}.jpg";
        var fileName = Path.GetFileName(path);
        var content = await System.IO.File.ReadAllBytesAsync(path);
        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);

        string oneBigString = Encoding.ASCII.GetString(content);

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

            if (fileContent.Length > 1000000)
            {
                return BadRequest("Max file size must be less than 1MB");
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
}
