using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinKien.IBAN4Net;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Enums;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Swagger;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/ledgers")]
public class LedgersController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _config;
    public LedgersController(UserDbContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public Task<List<LedgerDto>> Get([FromQuery] int page = 0, [FromQuery] int perPage = 100)
    {
        if (perPage >  1000)
        {
            Response.Headers.Add("UnrestrictedConsumptionPagination(1)", _config["Flags:UnrestrictedConsumptionPagination(1)"]);
        }

        return _context.Ledgers
            .Where(l => l.User.Email == User.GetEmail())
            .Skip(page * perPage)
            .Take(perPage)
            .Select(l => MapLedger(l))
            .ToListAsync();
    }

    [Authorize]
    [HttpGet("{ledgerId}")]
    public async Task<IActionResult> Get(Guid ledgerId)
    {
        var ledger = await _context.Ledgers
            .Where(l => l.User.Email == User.GetEmail() && l.Id == ledgerId)
            .Select(l => MapLedger(l))
            .FirstOrDefaultAsync();

        if (ledger is null)
        {
            return NotFound();
        }

        return Ok(ledger);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLedgerDto ledgerDto)
    {
        var legerAlreadyExists = await _context.Ledgers
            .Where(l => l.User.Email == User.GetEmail())
            .AnyAsync(l => l.Currency == ledgerDto.Currency);
        if (legerAlreadyExists)
        {
            return BadRequest($"Ledger for currency {ledgerDto.Currency} already created.");
        }

        var userEmail = User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        var user = await _context.Users.FirstAsync(u => u.Email == userEmail);

        var ledgerId = Guid.NewGuid();
        var ledger = new Ledger
        {
            Id = Guid.NewGuid(),
            Currency = ledgerDto.Currency,
            Balance = 0,
            Iban = IbanGenerator.GenerateIban(),
            UserId = user.Id,
            BalanceLimit = ledgerDto.BalanceLimit
        };
        _context.Ledgers.Add(ledger);

        await _context.SaveChangesAsync();

        if (ledgerDto.BalanceLimit != 900)
        {
            Response.Headers.Add("BrokenObjectPropertyAuthorizationInternalProperty", _config["Flags:BrokenObjectPropertyAuthorizationInternalProperty"]);
        }

        return Created( $"ledgers/{ledgerId}",MapLedger(ledger));
    }

    public record CreateLedgerDto(Currency Currency)
    {
        [SwaggerIgnore]
        public double BalanceLimit { get; set; } = 900;
    };

    public record LedgerDto(Guid Id, Currency Currency, double Balance, string Iban, double BalanceLimit);

    private static LedgerDto MapLedger(Ledger ledger)
    {
        return new LedgerDto(ledger.Id, ledger.Currency, ledger.Balance, ledger.Iban, ledger.BalanceLimit);
    }

}
