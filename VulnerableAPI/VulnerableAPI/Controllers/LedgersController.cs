using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        var ledgerId = Guid.NewGuid();
        _context.Ledgers.Add(new Ledger
        {
            Id = Guid.NewGuid(),
            Currency = Currency.EUR,
            Balance = 100
        });
        await _context.SaveChangesAsync();
        return Ok(ledgerId);
    }
}
