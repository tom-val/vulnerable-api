using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Enums;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/friends")]
public class FriendsController : ControllerBase
{
    private readonly UserDbContext _context;
    public FriendsController(UserDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public Task<List<FriendDto>> Get()
    {
        return _context.Friends
            .Where(l => l.User.Email == User.GetEmail())
            .Select(favorite => new FriendDto(favorite.Id, favorite.FriendUser.FirstName, favorite.FriendUser.Email, favorite.Name, _context.Ledgers.Where(l => l.UserId == favorite.FriendUserId).Select(l => l.Currency).ToList()))
            .ToListAsync();
    }

    public record FriendDto(Guid id, string FirstName, string Email, string Name, List<Currency> Currencies);
}
