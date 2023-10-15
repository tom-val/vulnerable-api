using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnerableAPI.Database;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/send-money")]
public class SendMoneyController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public SendMoneyController(UserDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendMoney(SendMoneyDto sendMoneyDto)
    {
        var moneyToLedger = await _context.Ledgers
            .FirstOrDefaultAsync(m => m.Iban == sendMoneyDto.Iban);

        if (moneyToLedger is null)
        {
            return NotFound();
        }

        if (moneyToLedger.UserId == User.GetId())
        {
            return BadRequest("Cannot send money to yourself.");
        }

        await using var dbContextTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var requestedFromLedger = _context.Ledgers.First(l => l.UserId == User.GetId() && l.Currency == moneyToLedger.Currency);

        if (requestedFromLedger.Balance < sendMoneyDto.Amount)
        {
            return BadRequest($"Not enough money in currency {requestedFromLedger.Currency} to send money.");
        }

        requestedFromLedger.Balance -= sendMoneyDto.Amount;
        moneyToLedger.Balance += sendMoneyDto.Amount;
        await _context.SaveChangesAsync();
        await dbContextTransaction.CommitAsync();

        return Ok(new {TransferFrom = requestedFromLedger.Id, TranserTo = moneyToLedger.Id, Amount = sendMoneyDto.Amount, RemainingAmount = requestedFromLedger.Balance});
    }

    public record SendMoneyDto(string Iban, [Range(0, int.MaxValue)] double Amount);
}
