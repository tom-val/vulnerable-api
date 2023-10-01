using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Enums;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/money-requests")]
public class MoneyRequestController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public MoneyRequestController(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpGet("requested-by-me")]
    public Task<List<MyMoneyRequestDto>> GetRequestedByMe()
    {
        return _context.MoneyRequests
            .Where(l => l.RequestedBy.Email == User.GetEmail())
            .Select(request => new MyMoneyRequestDto(request.Id, request.Status, request.RequestedFrom.FirstName, request.RequestReason))
            .ToListAsync();
    }

    [Authorize]
    [HttpGet("requested-from-me")]
    public Task<List<MoneyRequestFromMeDto>> GetRequestedFromMe()
    {
        return _context.MoneyRequests
            .Where(l => l.RequestedFrom.Email == User.GetEmail())
            .Select(request => new MoneyRequestFromMeDto(request.Id, request.Status, request.RequestedBy.FirstName, request.RequestReason))
            .ToListAsync();
    }

    [Authorize]
    [HttpPost("{moneyRequestId}/accept")]
    public async Task<IActionResult> Accept(Guid moneyRequestId)
    {
        var moneyRequest = await _context.MoneyRequests
            .Include(m => m.RequestedBy)
            .FirstOrDefaultAsync(m => m.Id == moneyRequestId);
        if (moneyRequest is null)
        {
            return NotFound();
        }

        await using var dbContextTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var moneyToLedger = _context.Ledgers.First(l => l.UserId == moneyRequest.RequestedById && l.Currency == moneyRequest.Currency);
        var requestedFromLedger = _context.Ledgers.First(l => l.UserId == moneyRequest.RequestedFromId && l.Currency == moneyRequest.Currency);

        if (requestedFromLedger.Balance < moneyRequest.Amount)
        {
            return BadRequest($"Not enough money in currency {moneyRequest.Currency} to send money");
        }

        requestedFromLedger.Balance -= moneyRequest.Amount;
        moneyToLedger.Balance += moneyRequest.Amount;
        moneyRequest.Status = MoneyRequestStatus.Approved;
        await _context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();

        if (moneyRequest.RequestedBy.Email == User.GetEmail())
        {
            Response.Headers.Add("BrokenObjectLevelAuthorization", _config["Flags:BrokenObjectLevelAuthorization"]);
        }

        return Ok(new MoneyRequestFromMeDto(moneyRequest.Id, moneyRequest.Status, moneyRequest.RequestedBy.FirstName, moneyRequest.RequestReason));
    }

    [Authorize]
    [HttpPost("{moneyReuqestId}/decline")]
    public async Task<IActionResult> Decline(Guid moneyRequestId)
    {
        var moneyRequest = await _context.MoneyRequests
            .Where(l => l.RequestedBy.Email == User.GetEmail())
            .FirstOrDefaultAsync(m => m.Id == moneyRequestId);
        if (moneyRequest is null)
        {
            return NotFound();
        }

        moneyRequest.Status = MoneyRequestStatus.Declined;
        await _context.SaveChangesAsync();
        return Ok(new MoneyRequestFromMeDto(moneyRequest.Id, moneyRequest.Status, moneyRequest.RequestedBy.FirstName, moneyRequest.RequestReason));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RequestMoney([FromBody] CreateMoneyRequestDto requestDto)
    {
        //TODO Could return friend balance here (find out which wrong)
        if (requestDto.RequestFromEmail == User.GetEmail())
        {
            return BadRequest("Cannot request money from yourself.");
        }

        var friend = await _context.Friends
            .Where(l => l.User.Email == User.GetEmail() && l.FriendUser.Email == requestDto.RequestFromEmail)
            .FirstOrDefaultAsync();

        if (friend is null)
        {
            return BadRequest("Can request money only from friends.");
        }

        var hasCorrectLedger = await _context.Ledgers.AnyAsync(l => l.UserId == friend.FriendUserId && l.Currency == requestDto.Currency);
        if (!hasCorrectLedger)
        {
            return BadRequest("Friend does not support this currency.");
        }

        var moneyRequest = new MoneyRequest
        {
            Id = Guid.NewGuid(),
            Amount = requestDto.Amount,
            Currency = requestDto.Currency,
            RequestedById = User.GetId(),
            RequestedFromId = friend.FriendUserId,
            RequestReason = requestDto.RequestReason,
            Status = MoneyRequestStatus.Requested
        };

        _context.MoneyRequests.Add(moneyRequest);
        await _context.SaveChangesAsync();
        return Ok(new { moneyRequest.Id, moneyRequest.Status, moneyRequest.RequestReason});
    }

    public record MyMoneyRequestDto(Guid Id, MoneyRequestStatus Status, string RequestedFrom, string RequestReason);
    public record MoneyRequestFromMeDto(Guid Id, MoneyRequestStatus Status, string RequestedBy, string RequestReason);
    public record CreateMoneyRequestDto([EmailAddress] string RequestFromEmail, double Amount, Currency Currency, string RequestReason);
}
