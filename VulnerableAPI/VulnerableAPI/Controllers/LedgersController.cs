using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SinKien.IBAN4Net;
using VulnerableAPI.Database;
using VulnerableAPI.Database.Models;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/ledgers")]
public class LedgersController : ControllerBase
{
    private readonly DatabaseContext _context;
    public LedgersController(CreatedDbContext context)
    {
        _context = context.Context;
    }

    [Authorize]
    [HttpGet]
    public Task<List<Ledger>> Get()
    {
        return _context.Ledgers.ToListAsync();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var iban = new IbanBuilder()
            .CountryCode(CountryCode.GetCountryCode( "LT" ))
            .BankCode("654")
            .AccountNumber(GenerateRandomAccountNumber())
            .Build();
        var ledgerId = Guid.NewGuid();
        _context.Ledgers.Add(new Ledger
        {
            Id = Guid.NewGuid(),
            Currency = Currency.EUR,
            Balance = 100,
            Iban = iban.ToString()
        });
        await _context.SaveChangesAsync();
        return Ok(ledgerId);
    }

    private string GenerateRandomAccountNumber()
    {
        var number = new StringBuilder();
        var random = new Random();
        number.Append(random.Next(100, 999));
        return number.ToString();
    }
}
