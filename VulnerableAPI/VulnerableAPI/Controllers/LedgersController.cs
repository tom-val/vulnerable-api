using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinKien.IBAN4Net;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/ledgers")]
public class LedgersController : ControllerBase
{
    private readonly UserDbContext _context;
    public LedgersController(UserDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public Task<List<LedgerDto>> Get()
    {
        return _context.Ledgers
            .Where(l => l.User.Email == User.GetEmail())
            .Select(l => MapLedger(l))
            .ToListAsync();
    }

    [Authorize]
    [HttpGet("{ledgerId}")]
    public async Task<IActionResult> Get(Guid ledgerId)
    {
        var ledger = await _context.Ledgers
            .Where(l => l.User.Email == User.GetEmail())
            .Select(l => MapLedger(l))
            .FirstOrDefaultAsync(l => l.Id == ledgerId);

        if (ledger is null)
        {
            return NotFound();
        }

        return Ok(ledger);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        var user = await _context.Users.FirstAsync(u => u.Email == userEmail);

        var ledgerId = Guid.NewGuid();
        var ledger = new Ledger
        {
            Id = Guid.NewGuid(),
            Currency = Currency.EUR,
            Balance = 0,
            Iban = IbanGenerator.GenerateIban(),
            UserId = user.Id
        };
        _context.Ledgers.Add(ledger);

        await _context.SaveChangesAsync();
        return Created( $"ledgers/{ledgerId}",MapLedger(ledger));
    }

    public record LedgerDto(Guid Id, Currency Currency, double Balance, string Iban);

    private static LedgerDto MapLedger(Ledger ledger)
    {
        return new LedgerDto(ledger.Id, ledger.Currency, ledger.Balance, ledger.Iban);
    }

}
